

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/GameObject Event Listener", fileName = "GameObject Event Listener")]
public class GameObjectGameEventListener : BaseGameEventListener<GameObject>
{
    [SerializeField] protected GameObjectGameEvent OnEvent = default;

    public override void Register(Action<GameObject> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<GameObject> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
