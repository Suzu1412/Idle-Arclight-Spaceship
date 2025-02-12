using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Binding/Void Event Binding", fileName = "Void Event Binding")]
public class VoidGameEventBinding : ScriptableObject, IGameEventBindingEditorHelper
{
    [SerializeField] protected VoidGameEvent gameEvent;

    // Track individual MonoBehaviours per GameObject
    protected Dictionary<GameObject, HashSet<MonoBehaviour>> subscribers = new();

    // Expose GameObjects for Editor Debugging
    public List<GameObject> SubscribedGameObjects => new List<GameObject>(subscribers.Keys);

    public void Bind(Action listener, MonoBehaviour owner)
    {
        if (listener == null || owner == null) return;

        if (!subscribers.ContainsKey(owner.gameObject))
        {
            subscribers[owner.gameObject] = new HashSet<MonoBehaviour>();
        }

        // Only register if this specific script hasn't been added
        if (subscribers[owner.gameObject].Add(owner))
        {
            gameEvent?.Register(listener);
        }
    }

    public void Unbind(Action listener, MonoBehaviour owner)
    {
        if (listener == null || owner == null) return;

        if (subscribers.ContainsKey(owner.gameObject))
        {
            // Remove the script from the set
            subscribers[owner.gameObject].Remove(owner);

            // If no more scripts are using it, remove the GameObject from the dictionary
            if (subscribers[owner.gameObject].Count == 0)
            {
                subscribers.Remove(owner.gameObject);
            }

            gameEvent?.Unregister(listener);
        }
    }

    public void ClearAllBindings()
    {
        subscribers.Clear();
    }

    public List<GameObject> GetSubscribedGameObjects()
    {
        return new List<GameObject>(subscribers.Keys);
    }

}
