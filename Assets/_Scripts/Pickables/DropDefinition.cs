using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropDefinition
{
    [SerializeField] private ObjectPoolSettingsSO _itemPool;
    [Range(0f, 1f)]
    [SerializeField] private float _dropChance;

    public ObjectPoolSettingsSO ItemPool => _itemPool;
    public float DropChance => _dropChance;
}
