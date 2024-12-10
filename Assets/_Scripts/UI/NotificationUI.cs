using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private RandomEventGameEventListener OnNotificateRandomEventListener;
    [SerializeField] private Transform _notificationParent;
    [SerializeField] protected ObjectPoolSettingsSO _activeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _completeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _randomEventPool;
    
    [SerializeField] private Transform _shopMessagePanelParent;
    [SerializeField] protected ObjectPoolSettingsSO _shopNotificationPool;

    private void OnEnable()
    {
        OnNotificateRandomEventListener.Register(NotifyRandomEvent);
    }

    private void OnDisable()
    {
        OnNotificateRandomEventListener.DeRegister(NotifyRandomEvent);
    }

    private void NotifyRandomEvent(BaseRandomEventSO randomEvent)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_randomEventPool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_notificationParent, false);
        message.SetRandomEvent(randomEvent);
    }


    private void NotifyShopEvent(INotification notification)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_shopNotificationPool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_shopMessagePanelParent, worldPositionStays: false);
        message.SetShopMessage(notification);
    }
}
