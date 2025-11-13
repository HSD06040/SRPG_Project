using System.Collections.Generic;
using System;

public class DiContainer
{
    private readonly Dictionary<Type, Func<object>> _bindings = new();
    private readonly Dictionary<Type, object> _singletons = new();

    public void Bind<TContract>(Func<object> factory)
    {
        _bindings[typeof(TContract)] = factory;
    }

    public TContract Resolve<TContract>()
    {
        return (TContract)Resolve(typeof(TContract));
    }

    private object Resolve(Type contractType)
    {
        if (_singletons.TryGetValue(contractType, out var instance))
        {
            return instance;
        }

        if (_bindings.TryGetValue(contractType, out var factory))
        {
            var newInstance = factory.Invoke();

            return newInstance;
        }

        throw new KeyNotFoundException($"Binding not found for type {contractType.Name}");
    }
}