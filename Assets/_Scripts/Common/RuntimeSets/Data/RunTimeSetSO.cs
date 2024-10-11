using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    private List<T> _items = new();
    public List<T> Items => _items;
    public Action ItemsChanged;

    public void Add(T item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            ItemsChanged?.Invoke();
        }
    }

    public void Remove(T item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            ItemsChanged?.Invoke();
        }
    }

    public T GetFirstItem()
    {
        return _items[0];
    }

    public T GetLastItem()
    {
        return _items[_items.Count - 1];
    }

    public List<T> GetAllItems() => _items;

    public T GetRandomItem()
    {
        return _items[UnityEngine.Random.Range(0, _items.Count)];
    }

    public T GetItemIndex(int index)
    {
        return _items[index];
    }
}