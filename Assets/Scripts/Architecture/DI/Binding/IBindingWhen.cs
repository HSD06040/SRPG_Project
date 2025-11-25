using System;
using UnityEngine;

namespace HSD.DI
{
    public class BindingFrom
    {
        readonly DiContainer _container;
        readonly BindInfo _info;

        public BindingFrom(DiContainer container, Type type)
        {
            _container = container;

            _info = new BindInfo
            {
                ContractType = type,
                ToType = type,
                Scope = ScopeTypes.Transient
            };

            _container.ReBinding(_info);
        }

        public BindingFrom AsSingle()
        {            
            _info.Scope = ScopeTypes.Singleton;
            _container.ReBinding(_info);

            return this;
        }

        public BindingFrom AsTransient()
        {
            _info.Scope = ScopeTypes.Transient;
            _container.ReBinding(_info);

            return this;
        }

        public BindingFrom To<TConcrete>()
        {
            _info.ToType = typeof(TConcrete);
            _container.ReBinding(_info);

            return this;
        }

        public BindingFrom DontDestory()
        {
            _info.DontDestory = true;
            _container.ReBinding(_info);

            return this;
        }

#region WithArguments
        public BindingFrom WithArguments<T>(T param)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param));
            return this;
        }

        public BindingFrom WithArguments<TParam1, TParam2>(
            TParam1 param1, TParam2 param2)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param1));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param2));
            return this;
        }

        public BindingFrom WithArguments<TParam1, TParam2, TParam3>(
            TParam1 param1, TParam2 param2, TParam3 param3)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param1));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param2));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param3));
            return this;
        }

        public BindingFrom WithArguments<TParam1, TParam2, TParam3, TParam4>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param1));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param2));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param3));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param4));
            return this;
        }

        public BindingFrom WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param1));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param2));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param3));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param4));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param5));
            return this;
        }

        public BindingFrom WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            _info.Arguments.Clear();
            _info.Arguments.Add(InjectUtil.CreateTypePair(param1));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param2));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param3));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param4));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param5));
            _info.Arguments.Add(InjectUtil.CreateTypePair(param6));
            return this;
        }
#endregion

    }
}
