using System;
using UnityEngine;

public class SaveSystem : PersistentSingleton<SaveSystem>
{
    [SerializeField] private GameData GameData;
    [SerializeField] private SaveableRunTimeSetSO _saveDataRTS = default;
    private const string _saveDataPath = "SaveSystem/";

    IDataService _dataService;

    protected override void Awake()
    {
        base.Awake();

        _dataService = new FileDataService(new JsonSerializer());
    }

    private void Start()
    {
        LoadGame(GameData.Name);
    }

    public void LoadGame(string gameName)
    {
        GameData = _dataService.Load(gameName);

        if (String.IsNullOrWhiteSpace(GameData.CurrentLevelName))
        {
            GameData.CurrentLevelName = "Demo";
        }

        if (_saveDataRTS == null)
        {
            Debug.LogError($"{_saveDataRTS} Not Present. Fix");
            return;
        }

        for (int i = 0; i < _saveDataRTS.Items.Count; i++)
        {
            _saveDataRTS.Items[i].LoadData(GameData);
        }

        //SceneManager.LoadScene(gameData.CurrentLevelName);
    }

    public void SaveGame()
    {
        if (_saveDataRTS == null)
        {
            Debug.LogError($"{_saveDataRTS} Not Present. Fix");
            return;
        }

        for (int i = 0; i < _saveDataRTS.Items.Count; i++)
        {
            _saveDataRTS.Items[i].SaveData(GameData);
        }

        //Debug.Log($"Dinero actual: {GameData.CurrencyData.Currency}");
        _dataService.Save(GameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
