using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Stat Info")]
public class StatInfoSO : ScriptableObject
{
    [SerializeField] private StatType _statType;
    [SerializeField] private float _minValue = 1;
    [SerializeField] private float _maxValue = 100;

    public StatType StatType { get => _statType; internal set => _statType = value; }
    public float MinValue { get => _minValue; internal set => _minValue = value; }
    public float MaxValue { get => _maxValue; internal set => _maxValue = value; }
}
