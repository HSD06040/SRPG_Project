using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HSD.DI
{
    public struct TypeValuePair
    {
        public Type Type;
        public object Value;

        public TypeValuePair(Type type, object value)
        {
            Type = type;
            Value = value;
        }
    }

    public static class InjectUtil
    {
        public static TypeValuePair CreateTypePair<T>(T param)
        {
            return new TypeValuePair(
                param == null ? typeof(T) : param.GetType(), param);
        }

        public static List<TypeValuePair> CreateArgListExplicit<T>(T param)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param)
            };
        }

        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param1),
                CreateTypePair(param2)
            };
        }

        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3>(
            TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param1),
                CreateTypePair(param2),
                CreateTypePair(param3)
            };
        }

        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param1),
                CreateTypePair(param2),
                CreateTypePair(param3),
                CreateTypePair(param4)
            };
        }

        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param1),
                CreateTypePair(param2),
                CreateTypePair(param3),
                CreateTypePair(param4),
                CreateTypePair(param5)
            };
        }

        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            return new List<TypeValuePair>
            {
                CreateTypePair(param1),
                CreateTypePair(param2),
                CreateTypePair(param3),
                CreateTypePair(param4),
                CreateTypePair(param5),
                CreateTypePair(param6)
            };
        }
    }

}