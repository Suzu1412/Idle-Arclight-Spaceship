using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseGameEvent<T> : ScriptableObject
{
    private event Action<T> OnEventRaised;

    /// <summary>
    /// Raise Events will tell every observer that something has changed
    /// Example: The Health Amount has been modified will tell UI or related scripts that something has changed
    /// </summary>
    public void RaiseEvent(T parameter)
    {
        OnEventRaised?.Invoke(parameter);
    }

    internal void Add(Action<T> onEvent)
    {
        OnEventRaised += onEvent;
    }

    internal void Remove(Action<T> onEvent)
    {
        OnEventRaised -= onEvent;
    }
}
