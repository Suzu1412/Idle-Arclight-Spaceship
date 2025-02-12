using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Vector2 Event Listener", fileName = "Vector2 Event Listener")]
public class Vector2GameEventListener : BaseGameEventListener<Vector2>
{
    [SerializeField] protected Vector2GameEvent OnEvent = default;

    public override void Register(Action<Vector2> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<Vector2> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
