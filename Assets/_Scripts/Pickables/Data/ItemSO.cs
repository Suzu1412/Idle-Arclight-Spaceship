using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/")]
public abstract class ItemSO : ScriptableObject
{
    [SerializeField] protected bool _isStackable;
    [SerializeField] protected int _maxStackSize = 1;
    [SerializeField] protected Sprite _itemImage;
    [TextArea(1, 20)] protected string _description;


    public string Name;
    public bool IsStackable => _isStackable;
    public int MaxStackSize => _maxStackSize;
    public Sprite ItemImage => _itemImage;
    public string Description => _description;

    public abstract void PickUp(IAgent agent);
}
