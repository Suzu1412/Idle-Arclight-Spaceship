using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour, ISaveable
{
    [SerializeField] private VoidGameEventListener OnProgressUnlocked;
    [SerializeField] private List<BoolVariableSO> _unlockedSystems;

    public void LoadData(GameData gameData)
    {
        foreach(var unlockedSystem in _unlockedSystems)
        {
            unlockedSystem.Value = false;
        }

    }

    public void SaveData(GameData gameData)
    {
    }
}
