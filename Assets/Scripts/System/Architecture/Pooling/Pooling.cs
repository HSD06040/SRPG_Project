using UnityEngine.Pool;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor;

public static class Pooling
{
    static readonly Dictionary<string, IObjectPool> pools = new Dictionary<string, IObjectPool>(100);    

    public static T Get<T>(T instance) where T : Component
    {
        return Object.Instantiate(instance);
    }
}

public struct PoolChain<T> where T : Component
{
    PoolInfo<T> poolInfo;

    public PoolChain()
    {
        poolInfo = new PoolInfo<T>();
    }

    public PoolChain<T> SetLifeTime(float lifeTime)
    {
        poolInfo.lifeTime = lifeTime;
        return this;
    }

    public PoolChain<T> SetMaxSize(int size)
    {
        poolInfo.customPoolSize = size;
        return this;
    }
}

struct PoolInfo<T> where T : Component
{
    public ObjectPool<T> pool;
    public float lifeTime;
    public bool autoRelease;
    public int customPoolSize;

    public void Creation()
    {

    }
}