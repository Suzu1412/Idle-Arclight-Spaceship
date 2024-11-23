using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour, ISaveable
{
    [SerializeField] private VoidGameEventListener OnProgressUnlocked;
    [SerializeField] private List<BoolVariableSO> _unlockedSystems;

    public void LoadData(GameDataSO gameData)
    {
    }

    public void SaveData(GameDataSO gameData)
    {
    }
}
