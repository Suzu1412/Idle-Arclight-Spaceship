

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Float Event Listener", fileName = "Float Event Listener")]
public class FloatGameEventListener : BaseGameEventListener<float>
{
    [SerializeField] protected FloatGameEvent OnEvent = default;

    public override void Register(Action<float> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<float> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
