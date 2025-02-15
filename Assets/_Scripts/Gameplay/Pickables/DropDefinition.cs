using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropDefinition
{
    [SerializeField] private ObjectPoolSettingsSO _itemPool;
    [Range(0f, 1f)]
    [SerializeField] private float _dropChance;
    [SerializeField] private bool _isCoin = false;

    public ObjectPoolSettingsSO ItemPool => _itemPool;
    public float DropChance => _dropChance;
    public bool IsCoin => _isCoin;
}
