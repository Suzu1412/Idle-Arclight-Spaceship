using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class ObjectPoolFactory : Singleton<ObjectPoolFactory>
{
    private readonly Dictionary<int, IObjectPool<ObjectPooler>> _pools = new();
    private readonly Dictionary<int, int> _childTransform = new();

    protected override void Awake()
    {
        base.Awake();

        int i = 0;

        foreach (string name in Enum.GetNames(typeof(ObjectPoolType)))
        {
            GameObject newChild = new(name);
            newChild.transform.SetParent(transform);
            _childTransform.Add((int)Enum.Parse(typeof(ObjectPoolType), name), i);
            i++;
        }
    }

    /// <summary>
    /// Used to retrieve the Child Transform of the Object Pooler
    /// Its purpose is to set it as the parent of the Created Pool Object
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal Transform GetChildTransformPosition(ObjectPoolType type)
    {
        if (_childTransform.TryGetValue((int)type, out int childPosition))
        {
            return transform.GetChild(childPosition);
        }

        return transform;
    }

    /// <summary>
    /// Spawn an Instance of an Object Pooler
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static ObjectPooler Spawn(ObjectPoolSettingsSO settings)
    {
        return Instance.GetPoolFor(settings)?.Get();
    }

    /// <summary>
    /// Return The object pool
    /// </summary>
    /// <param name="o">The Object Pooler - Better to cache it</param>
    public static void ReturnToPool(ObjectPooler o)
    {
        Instance.GetPoolFor(o.Settings)?.Release(o);
    }

    IObjectPool<ObjectPooler> GetPoolFor(ObjectPoolSettingsSO settings)
    {
        IObjectPool<ObjectPooler> pool;

        int poolKey = settings.Prefab.GetInstanceID();

        if (_pools.TryGetValue(poolKey, out pool)) return pool;

        pool = new ObjectPool<ObjectPooler>(
            settings.Create, // Create required GameObject
            settings.OnGet, // Get the Game Object on the Pool
            settings.OnRelease, // Store Game object on the pool
            settings.OnDestroyPoolObject, // Destroy once is over Max Pool Size
            settings.CollectionCheck, // Ensures only saved once on the Pool -> should be True
            settings.DefaultCapacity, // The expected capacity - initial capacity
            settings.MaxPoolSize // After max achieved destroy new objects on pool
            );

        _pools.Add(poolKey, pool);
        return pool;
    }
}
