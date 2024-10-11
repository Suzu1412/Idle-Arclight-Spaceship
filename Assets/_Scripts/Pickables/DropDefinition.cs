using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropDefinition
{
    [SerializeField] private ItemSO _item;
    [Range(0f, 1f)]
    [SerializeField] private float _dropChance;

    public ItemSO Item => _item;
    public float DropChance => _dropChance;
}
