using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatModifier
{
    StatModifierType ModifierType { get; }
    StatType StatType { get; }
    float Value { get; }
    string Source { get; }
}
