using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class GameManager : Singleton<GameManager>, ISaveable
{
    private bool _hasLoadedGame = false;


    #region Save System
    public void SaveData(GameData gameData)
    {
    }

    public void LoadData(GameData gameData)
    {
    }
    #endregion
}

