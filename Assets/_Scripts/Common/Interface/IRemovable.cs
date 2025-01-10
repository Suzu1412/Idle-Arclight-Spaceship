using System;
using UnityEngine;

public interface IRemovable
{
    event Action OnRemove;
    void Remove(GameObject source);
}
