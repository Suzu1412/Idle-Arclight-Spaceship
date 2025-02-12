using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Random Event Listener", fileName = "Random Event Listener")]
public class RandomEventGameEventListener : BaseGameEventListener<BaseRandomEventSO>
{
    [SerializeField] protected RandomEventGameEvent OnEvent = default;

    public override void Register(Action<BaseRandomEventSO> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<BaseRandomEventSO> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}