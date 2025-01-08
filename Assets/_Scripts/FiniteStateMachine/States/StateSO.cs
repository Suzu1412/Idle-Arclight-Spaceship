using System;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    [SerializeField] protected int phase => 0;
    [SerializeField] protected float _highestUtility = 0;

    public int Phase => phase;
    public float HighestUtility => _highestUtility;

    public abstract StateContext CreateContext();
}

public abstract class StateSO<T> : StateSO where T : StateContext, new()
{
    public override StateContext CreateContext()
    {
        return new T();
    }
}
