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

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
