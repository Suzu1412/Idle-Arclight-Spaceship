using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    void Initialize(int maxValue, int minPossibleValue, int maxPossiblevalue);

    int MaxValue { get; }

    int CurrentValue { get; }
}
