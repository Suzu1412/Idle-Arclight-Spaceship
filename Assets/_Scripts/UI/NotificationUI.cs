using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private RandomEventGameEventListener OnNotificateRandomEventListener;
    [SerializeField] private NotificationGameEventListener OnShopNotificationEventListener;
    [SerializeField] private NotificationGameEventListener OnOfflineNotificationEventListener;
    [SerializeField] private Transform _notificationParent;
    [SerializeField] private ObjectPoolSettingsSO _shopMessagePool;
    [SerializeField] private Transform _shopMessagePanelParent;
    [SerializeField] protected ObjectPoolSettingsSO _activeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _completeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _randomEventPool;
    [SerializeField] protected ObjectPoolSettingsSO _offlineEventPool;


    private void OnEnable()
    {
        OnNotificateRandomEventListener.Register(NotifyRandomEvent);
        OnShopNotificationEventListener.Register(NotifyShopEvent);
        OnOfflineNotificationEventListener.Register(NotifyOfflineEvent);
    }

    private void OnDisable()
    {
        OnNotificateRandomEventListener.DeRegister(NotifyRandomEvent);
        OnShopNotificationEventListener.DeRegister(NotifyShopEvent);
        OnOfflineNotificationEventListener.DeRegister(NotifyOfflineEvent);


    }

    private void NotifyRandomEvent(BaseRandomEventSO randomEvent)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_randomEventPool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_notificationParent, worldPositionStays: false);
        message.SetRandomEvent(randomEvent);
    }

    private void NotifyShopEvent(INotification notification)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_shopMessagePool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_shopMessagePanelParent, worldPositionStays: false);
        message.SetShopMessage(notification);
    }

    private void NotifyOfflineEvent(INotification notification)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_offlineEventPool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_shopMessagePanelParent, worldPositionStays: false);
        message.SetOfflineMessage(notification);
    }
}
