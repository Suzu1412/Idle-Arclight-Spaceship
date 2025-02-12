

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Double Event Listener", fileName = "Double Event Listener")]
public class DoubleGameEventListener : BaseGameEventListener<double>
{
    [SerializeField] protected DoubleGameEvent OnEvent = default;

    public override void Register(Action<double> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<double> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
