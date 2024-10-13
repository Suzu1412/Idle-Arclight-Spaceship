using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseGameEventListener<T> : ScriptableObject
{
    //[SerializeField] protected BaseGameEvent<T> OnEvent = default;

    /// <summary>
    /// Use on Enable: Can suscribe to Methods like:
    /// OnVoidEventListener.Register(MethodName);
    /// </summary>
    /// <param name="onEvent"></param>
    public abstract void Register(Action<T> onEvent);

    /// <summary>
    /// Use on Disable: Always desuscribe from events to avoid errors. Example:
    /// OnVoidEventListener.DeRegister(MethodName);
    /// </summary>
    /// <param name="onEvent"></param>
    public abstract void DeRegister(Action<T> onEvent);
}
