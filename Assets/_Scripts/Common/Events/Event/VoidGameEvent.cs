using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Event/Void Event", fileName = "Void Event")]
public class VoidGameEvent : ScriptableObject
{
    private event UnityAction OnEventRaised;

    /// <summary>
    /// Raise Events will tell every observer that something has changed
    /// Example: The Health Amount has been modified will tell UI or related scripts that something has changed
    /// </summary>
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }

    internal void Add(UnityAction onEvent)
    {
        OnEventRaised += onEvent;
    }

    internal void Remove(UnityAction onEvent)
    {
        OnEventRaised -= onEvent;
    }
}