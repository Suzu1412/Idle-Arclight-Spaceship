using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSystem
{
    event Action<float, float> OnRequiredExpChanged;
    
    event Action<int> OnLevelGained;

    event Action<float> OnExpGained;
    
    void GainExperienceFlat(float expGained);

    float GetCurrentExp();

    float GetTotalExp();

    int GetCurrentLevel();
}
