using UnityEngine;

[System.Serializable]
public class GameData
{
    public string Name;
    private string _fileExtension = ".sav";
    public string FileExtension => _fileExtension;

    public string LocationId;
    public string CurrentLevelName;

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
