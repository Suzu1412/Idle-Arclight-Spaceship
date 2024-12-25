using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Bool Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;

    [Header("Run Time Set")]
    [SerializeField] private GameObjectRuntimeSetSO _generatorRTS;
    [SerializeField] private GameObjectRuntimeSetSO _upgradesRTS;

    [SerializeField] private GameObject _shopRedNotification;
    [SerializeField] private GameObject _shopBlueNotification;
    [SerializeField] private GameObject _upgradeShopNotification;
    [SerializeField] private GameObject _generatorShopNotification;
    [SerializeField] private Button _buyEverythingButton;



    private void OnEnable()
    {
        OnNotificateRandomEventListener.Register(NotifyRandomEvent);
        OnShopNotificationEventListener.Register(NotifyShopEvent);
        OnOfflineNotificationEventListener.Register(NotifyOfflineEvent);
        _generatorRTS.OnItemsChanged += ShowShopNotification;
        _upgradesRTS.OnItemsChanged += ShowShopNotification;
        ShowShopNotification();
    }

    private void OnDisable()
    {
        OnNotificateRandomEventListener.DeRegister(NotifyRandomEvent);
        OnShopNotificationEventListener.DeRegister(NotifyShopEvent);
        OnOfflineNotificationEventListener.DeRegister(NotifyOfflineEvent);
        _generatorRTS.OnItemsChanged -= ShowShopNotification;
        _upgradesRTS.OnItemsChanged -= ShowShopNotification;

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

    private void ShowShopNotification()
    {
        if (_shopRedNotification == null || _upgradeShopNotification == null || _shopBlueNotification == null)
        {
            return;
        }

        _shopRedNotification.SetActive(_upgradesRTS.Count > 0);
        _upgradeShopNotification.SetActive(_upgradesRTS.Count > 0);
        _buyEverythingButton.interactable = _upgradesRTS.Count > 0;

        _shopBlueNotification.SetActive(_generatorRTS.Count > 0);
        _generatorShopNotification.SetActive(_generatorRTS.Count > 0);


    }
}
