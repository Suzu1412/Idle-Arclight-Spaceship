using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class BaseGameEvent<T> : ScriptableObject, IGameEvent
{
    protected event Action<T> OnEventRaised;

#if UNITY_EDITOR
    protected List<string> _eventHistory = new List<string>();
    protected Dictionary<UnityEngine.Object, int> _senderCounts = new Dictionary<UnityEngine.Object, int>();

    public List<string> EventHistory => _eventHistory;
    public Dictionary<UnityEngine.Object, int> SenderCounts => _senderCounts;

    public bool HasParameterType => true;

    public Type ParameterType => typeof(T);
#endif

    /// <summary>
    /// Raise the event and notify all listeners.
    /// Needs to be passed 'This' as parameter to log who activated the event
    /// </summary>
    public void RaiseEvent(T parameter, UnityEngine.Object sender)
    {
        if (OnEventRaised != null)
        {
            try
            {
                OnEventRaised.Invoke(parameter);
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
    internal void Register(Action<T> onEvent)
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
    internal void Unregister(Action<T> onEvent)
    {
        if (onEvent == null) return;

        OnEventRaised -= onEvent;
    }

    protected void LogEvent(UnityEngine.Object sender)
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

    public void RaiseFromEditor(object value, UnityEngine.Object sender)
    {
        if (value is T typedValue)
        {
            RaiseEvent(typedValue, sender);
        }
        else
        {
            Debug.LogWarning($"Invalid type passed to {name}: {value}");
        }
    }
}
