using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private RandomEventGameEventListener OnNotificateRandomEventListener;
    [SerializeField] private NotificationGameEventListener OnShopNotificationEventListener;
    [SerializeField] private Transform _notificationParent;
    [SerializeField] private NotifyMessageUI _shopMessagePanel;
    [SerializeField] private Transform _shopMessagePanelParent;
    [SerializeField] protected ObjectPoolSettingsSO _activeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _completeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _randomEventPool;
    


    private void OnEnable()
    {
        OnNotificateRandomEventListener.Register(NotifyRandomEvent);
        OnShopNotificationEventListener.Register(NotifyShopEvent);
    }

    private void OnDisable()
    {
        OnNotificateRandomEventListener.DeRegister(NotifyRandomEvent);
        OnShopNotificationEventListener.DeRegister(NotifyShopEvent);

    }

    private void NotifyRandomEvent(BaseRandomEventSO randomEvent)
    {
        NotifyMessageUI message = ObjectPoolFactory.Spawn(_randomEventPool).GetComponent<NotifyMessageUI>();
        message.transform.SetParent(_notificationParent, worldPositionStays: false);
        message.SetRandomEvent(randomEvent);
    }

    private void NotifyShopEvent(INotification notification)
    {
        Debug.Log(message: "recibe evento");
        _shopMessagePanel.gameObject.SetActive(true);
        _shopMessagePanel.transform.SetParent(_shopMessagePanelParent, worldPositionStays: false);
        _shopMessagePanel.SetShopMessage(notification);
    }
}
