using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Event/Void Event", fileName = "Void Event")]
public class VoidGameEvent : ScriptableObject
{
    protected event Action OnEventRaised;

#if UNITY_EDITOR
    private List<string> _eventHistory = new List<string>();
    private Dictionary<UnityEngine.Object, int> _senderCounts = new Dictionary<UnityEngine.Object, int>();

    public List<string> EventHistory => _eventHistory;
    public Dictionary<UnityEngine.Object, int> SenderCounts => _senderCounts;
#endif

    /// <summary>
    /// Raise the event and notify all listeners.
    /// Needs to be passed 'This' as parameter to log who activated the event
    /// </summary>
    public void RaiseEvent(UnityEngine.Object sender)
    {
        if (OnEventRaised != null)
        {
            try
            {
                OnEventRaised.Invoke();

#if UNITY_EDITOR
                LogEvent(sender);
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while invoking event {name}: {e}");
            }
        }
        else
        {
            Debug.LogWarning($"Event {name} was raised but has no listeners.");
        }
    }

    /// <summary>
    /// Registers a listener for this event.
    /// </summary>
    internal void Register(Action onEvent)
    {
        if (onEvent == null) return;

        // Ensure the same method isn't added twice
        if (OnEventRaised != null && OnEventRaised.GetInvocationList().Contains(onEvent))
        {
            Debug.LogWarning($"⚠️ Event already registered: {onEvent.Method.Name} on {onEvent.Target}");
            return;
        }
        OnEventRaised += onEvent;
    }

    /// <summary>
    /// Unregisters a listener from this event.
    /// </summary>
    internal void Unregister(Action onEvent)
    {
        if (onEvent == null) return;

        OnEventRaised -= onEvent;
    }

    private void LogEvent(UnityEngine.Object sender)
    {
        if (!_senderCounts.ContainsKey(sender))
        {
            _senderCounts[sender] = 0;
        }
        _senderCounts[sender]++;

        _eventHistory.Insert(0, $"{sender} triggered event at {System.DateTime.Now}");
        if (_eventHistory.Count > 10) // Limit history size
            _eventHistory.RemoveAt(_eventHistory.Count - 1);
    }

    public void RaiseFromEditor(UnityEngine.Object sender)
    {
        RaiseEvent(sender);
    }
}