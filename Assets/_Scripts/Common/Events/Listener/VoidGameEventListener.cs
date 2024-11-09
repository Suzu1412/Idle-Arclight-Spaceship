using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/Void Event Listener", fileName = "Void Event Listener")]
public class VoidGameEventListener : ScriptableObject
{
    [SerializeField] protected VoidGameEvent OnEvent = default;

    public void Register(UnityAction onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public void DeRegister(UnityAction onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}