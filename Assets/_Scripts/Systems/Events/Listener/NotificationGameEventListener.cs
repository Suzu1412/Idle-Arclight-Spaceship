using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Notification Event Listener", fileName = "Notification Event Listener")]
public class NotificationGameEventListener : BaseGameEventListener<INotification>
{
    [SerializeField] protected NotificationGameEvent OnEvent = default;

    public override void Register(Action<INotification> onEvent)
    {
        OnEvent.Register(onEvent);
    }

    public override void DeRegister(Action<INotification> onEvent)
    {
        OnEvent.Unregister(onEvent);
    }
}
