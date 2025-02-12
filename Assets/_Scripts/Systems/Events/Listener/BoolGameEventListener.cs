

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Bool Event Listener", fileName = "Bool Event Listener")]
public class BoolGameEventListener : BaseGameEventListener<bool>
{
    [SerializeField] protected BoolGameEvent OnEvent = default;

    public override void Register(Action<bool> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<bool> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
