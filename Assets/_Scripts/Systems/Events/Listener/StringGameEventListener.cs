using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/String Event Listener", fileName = "String Event Listener")]
public class StringGameEventListener : BaseGameEventListener<string>
{
    [SerializeField] protected StringGameEvent OnEvent = default;

    public override void Register(Action<string> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<string> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}
