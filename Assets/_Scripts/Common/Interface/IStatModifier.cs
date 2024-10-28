using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatModifier
{
    ModifierType ModifierType { get; }
    StatType StatType { get; }
    float Value { get; }
    string Source { get; }
}
