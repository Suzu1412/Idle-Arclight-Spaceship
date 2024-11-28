using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Sound Event Listener", fileName = "Sound Event Listener")]
public class SoundGameEventListener : BaseGameEventListener<ISound>
{
    [SerializeField] protected SoundGameEvent OnEvent = default;

    public override void Register(Action<ISound> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<ISound> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}

