using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    public List<T> Items { get; } = new();
    public UnityAction OnItemsChanged;

    public void Add(T item)
    {
        if (!Items.Contains(item))
        {
            Items.Add(item);
            OnItemsChanged?.Invoke();
        }
    }

    public void Remove(T item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            OnItemsChanged?.Invoke();
        }
    }

    public T GetFirstItem()
    {
        return Items[0];
    }

    public T GetLastItem()
    {
        return Items[^1];
    }

    public T GetRandomItem()
    {
        return Items[Random.Range(0, Items.Count)];
    }

    public T GetItemIndex(int index)
    {
        return Items[index];
    }

    public int Count => Items.Count;
}