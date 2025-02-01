using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStatModifier 
{
    [SerializeField] private List<StatModifier> _statsModifiers = new();
    public List<StatModifier> StatsModifiers => _statsModifiers;
}
