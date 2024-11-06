using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/List Generator Event Listener", fileName = "List Generator Event Listener")]
public class ListGeneratorGameEventListener : BaseGameEventListener<List<GeneratorSO>>
{
    [SerializeField] protected ListGeneratorGameEvent OnEvent = default;

    public override void Register(Action<List<GeneratorSO>> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<List<GeneratorSO>> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}
