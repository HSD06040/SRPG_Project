using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HSD.DI
{
    public enum ScopeTypes
    {
        Unset,
        Transient,
        Singleton
    }    

    public class BindInfo
    {
        public ScopeTypes Scope;
        public Type ContractType;
        public Type ToType;
        public bool DontDestory;
        public readonly List<TypeValuePair> Arguments;

        public BindInfo()
        {
            Arguments = new List<TypeValuePair>();
        }
    }
}
