using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    [SerializeField] protected int phase = 0;
    [SerializeField] [Range(1f, 100f)] protected float _highestUtility = 0;
    [SerializeField] protected List<ActionSO> _actions = new(); 

    public int Phase => phase;
    public float HighestUtility => _highestUtility;
    public List<ActionSO> Actions => _actions;

    public abstract StateContext CreateContext();
}

public abstract class StateSO<T> : StateSO where T : StateContext, new()
{
    public override StateContext CreateContext()
    {
        return new T();
    }
}
