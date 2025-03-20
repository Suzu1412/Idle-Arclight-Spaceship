using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialGemConfigSO", menuName = "Scriptable Objects/SpecialGemConfigSO")]
public class SpecialGemConfigSO : ScriptableObject
{
    [SerializeField] private ObjectPoolSettingsSO _poolSettings;
    [SerializeField] private List<SpecialGemData> _specialGemDataList;


    // Method to calculate spawn chance
    // Method to add the gem data to the item
}

[System.Serializable]
public class SpecialGemData
{
    [SerializeField] private AnimatorOverrideController _animatorOverrideController;
    [SerializeField] private FloatVariableSO _gemstoneSpawnChance;
    // Gem Movement

    public AnimatorOverrideController AnimatorOverrideController;
    public float GemStoneSpawnChance => _gemstoneSpawnChance.Value;
}