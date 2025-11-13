using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace HSD.DI
{
    public class DiContainer
    {
        readonly static Dictionary<Type, object> _singletons = new();
        readonly static Dictionary<Type, BindInfo> _bindInfos = new();
        readonly List<BindInfo> _bindings = new List<BindInfo>();

        const BindingFlags k_bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public BindingFrom Bind<TContract>() where TContract : class
        {
            var bindingFrom = new BindingFrom(this, typeof(TContract));

            return bindingFrom;
        }

        public void RegisterBindings()
        {
            foreach (var binding in _bindings)
            {
                RegisterBinding(binding);
            }
        }

        void RegisterBinding(BindInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            if (info.Scope == ScopeTypes.Singleton)
            {
                CreateSingleInstance(info);
            }

            _bindInfos[info.ContractType] = info;
        }

        void CreateSingleInstance(BindInfo info)
        {
            if (_singletons.TryGetValue(info.ContractType, out var oldInstance))
            {
                if (oldInstance != null)
                    return;
            }

            if (typeof(MonoBehaviour).IsAssignableFrom(info.ToType))
            {

                Type componentType = info.ToType;
                GameObject obj = new GameObject($"[Singleton] {componentType.Name}");
                MonoBehaviour instance = (MonoBehaviour)obj.AddComponent(componentType);

                if (info.DontDestory)
                    UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);

                InjectMethodsWithArguments(instance, info.Arguments);

                _singletons[info.ContractType] = instance;
            }
            else
            {
                var newInstance = CreateInstanceWithArguments(info.ToType, info.Arguments);
                _singletons[info.ContractType] = newInstance;
            }
        }

        public object Resolve(Type contractType)
        {
            if (_singletons.TryGetValue(contractType, out var instance))
            {
                return instance;
            }

            if (_bindInfos.TryGetValue(contractType, out var info))
            {
                if (info.Scope == ScopeTypes.Transient)
                {
                    Type concreteType = info.ToType;
                    object newInstance = null;

                    if (typeof(MonoBehaviour).IsAssignableFrom(concreteType))
                    {
                        GameObject obj = new GameObject($"[Transient] {contractType.Name}");
                        newInstance = obj.AddComponent(concreteType);

                        // MonoBehaviour 메서드 주입
                        InjectMethodsWithArguments(newInstance, info.Arguments);
                    }
                    else
                    {
                        // 일반 클래스 생성자 주입
                        newInstance = CreateInstanceWithArguments(concreteType, info.Arguments);
                    }

                    if (newInstance != null)
                    {
                        return newInstance;
                    }
                }
            }

            return null;
        }

        public void ReBinding(BindInfo bindingFrom)
        {
            int index = _bindings.FindIndex(b => b == bindingFrom);

            if (index != -1)
            {
                _bindings[index] = bindingFrom;
            }
            else
            {
                _bindings.Add(bindingFrom);
            }
        }
        object CreateInstanceWithArguments(Type type, List<TypeValuePair> arguments)
        {
            if (arguments == null || arguments.Count == 0)
            {
                return Activator.CreateInstance(type);
            }

            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            ConstructorInfo injectConstructor = null;

            foreach (var ctor in constructors)
            {
                if (Attribute.IsDefined(ctor, typeof(InjectAttribute)))
                {
                    injectConstructor = ctor;
                    break;
                }
            }

            if (injectConstructor == null)
            {
                injectConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == arguments.Count);
            }

            if (injectConstructor == null)
            {
                return Activator.CreateInstance(type);
            }

            var parameters = injectConstructor.GetParameters();
            var paramValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];

                var matchingArg = arguments.FirstOrDefault(arg =>
                    param.ParameterType.IsAssignableFrom(arg.Type));

                if (matchingArg.Value != null)
                {
                    paramValues[i] = matchingArg.Value;
                }
                else
                {
                    paramValues[i] = Resolve(param.ParameterType);
                }
            }

            return injectConstructor.Invoke(paramValues);
        }
        void InjectMethodsWithArguments(object instance, List<TypeValuePair> arguments)
        {
            var type = instance.GetType();
            var injectableMethods = type.GetMethods(k_bindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var parameters = injectableMethod.GetParameters();
                var paramValues = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];

                    var matchingArg = arguments?.FirstOrDefault(arg =>
                        param.ParameterType.IsAssignableFrom(arg.Type));

                    if (matchingArg.HasValue && matchingArg.Value.Value != null)
                    {
                        paramValues[i] = matchingArg.Value.Value;
                    }
                    else
                    {
                        paramValues[i] = Resolve(param.ParameterType);

                        if (paramValues[i] == null)
                        {
                            throw new Exception($"Failed to resolve parameter '{param.Name}' of type '{param.ParameterType.Name}' for method '{injectableMethod.Name}' in class '{type.Name}'.");
                        }
                    }
                }

                injectableMethod.Invoke(instance, paramValues);
            }
        }
        bool IsInjectable(MonoBehaviour obj)
        {
            var type = obj.GetType();

            var isContainerSingleton = _bindInfos.Values
                .Any(info =>
                    info.Scope == ScopeTypes.Singleton &&
                    info.ToType == type);

            if (isContainerSingleton)
            {
                return false;
            }

            return HasInjectableMembers(obj);
        }

#region Inject
        public void Binding()
        {
            var monoBehaviours = FindMonoBehaviours();

            var injectables = monoBehaviours.Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        public void Register<T>(T instance)
        {
            _singletons[typeof(T)] = instance;
        }

        public void Inject(object instance)
        {
            var type = instance.GetType();

            // Field 주입
            InjectFields(instance, type);

            // Method 주입
            InjectMethods(instance, type);

            // Property 주입
            InjectPropertys(instance, type);
        }

        private void InjectPropertys(object instance, Type type)
        {
            var injectableProperties = type.GetProperties(k_bindingFlags)
                            .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }

                injectableProperty.SetValue(instance, resolvedInstance);
            }
        }

        private void InjectMethods(object instance, Type type)
        {
            var injectableMethods = type.GetMethods(k_bindingFlags)
                            .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Failed to inject dependencies into method '{injectableMethod.Name}' of class '{type.Name}'.");
                }

                injectableMethod.Invoke(instance, resolvedInstances);
            }
        }

        private void InjectFields(object instance, Type type)
        {
            var injectableFields = type.GetFields(k_bindingFlags)
                            .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                if (injectableField.GetValue(instance) != null)
                {
                    Debug.LogWarning($"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject dependency into field '{injectableField.Name}' of class '{type.Name}'.");
                }

                injectableField.SetValue(instance, resolvedInstance);
            }
        }
        #endregion

#region static
        static MonoBehaviour[] FindMonoBehaviours()
        {
            return UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
        static bool HasInjectableMembers(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(k_bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
#endregion

    }
}