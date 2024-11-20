using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private RandomEventGameEventListener OnNotificateRandomEventListener;
    [SerializeField] private Transform _notificationParent;
    [SerializeField] protected ObjectPoolSettingsSO _activeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _completeQuestPool;
    [SerializeField] protected ObjectPoolSettingsSO _randomEventPool;
    


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
}
