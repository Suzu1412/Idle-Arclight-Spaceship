using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Int Event Listener", fileName = "Int Event Listener")]
public class IntGameEventListener : BaseGameEventListener<int>
{
    [SerializeField] protected IntGameEvent OnEvent = default;

    public override void Register(Action<int> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<int> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
