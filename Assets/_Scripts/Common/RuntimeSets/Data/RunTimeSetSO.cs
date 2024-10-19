using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    private readonly List<T> _items = new();
    public List<T> Items => _items;
    public UnityAction OnItemsChanged;

    public void Add(T item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            OnItemsChanged?.Invoke();
        }
    }

    public void Remove(T item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            OnItemsChanged?.Invoke();
        }
    }

    public T GetFirstItem()
    {
        return _items[0];
    }

    public T GetLastItem()
    {
        return _items[^1];
    }

    public T GetRandomItem()
    {
        return _items[Random.Range(0, _items.Count)];
    }

    public T GetItemIndex(int index)
    {
        return _items[index];
    }
}