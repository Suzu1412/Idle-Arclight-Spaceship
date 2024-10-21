using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string Name;

    public string LocationId;
    public string CurrentLevelName;

    [Header("Game Manager")]
    public GameStateType CurrentGameState;

    [Header("Currency")]
    public CurrencyData CurrencyData;
    public List<GeneratorData> GeneratorsData;

    public void NewGame(string name)
    {
        Name = name;

        CurrentGameState = GameStateType.Init;
        CurrencyData = new CurrencyData();
        GeneratorsData = new List<GeneratorData>();
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
