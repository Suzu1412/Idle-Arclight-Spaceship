using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/String Event Listener", fileName = "String Event Listener")]
public class StringGameEventListener : BaseGameEventListener<string>
{
    [SerializeField] protected StringGameEvent OnEvent = default;

    public override void Register(Action<string> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<string> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
