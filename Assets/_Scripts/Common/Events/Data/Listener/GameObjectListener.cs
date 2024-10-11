

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/GameObject Event Listener", fileName = "GameObject Event Listener")]
public class GameObjectGameEventListener : BaseGameEventListener<GameObject>
{
    [SerializeField] protected GameObjectGameEvent OnEvent = default;

    public override void Register(Action<GameObject> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<GameObject> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}
