using System;
using UnityEngine;

public class SaveSystem : PersistentSingleton<SaveSystem>
{
    [SerializeField] private GameDataSO _gameDataSO = default;
    private GameData _gameData;
    private GameData _fileGameData;
    //private GameData _cloudGameData;
    [SerializeField] private SaveableRunTimeSetSO _saveDataRTS = default;
    private const string _saveDataPath = "SaveSystem/";

    IDataService _dataService;
    //ICloudService _cloudDataService;

    protected override void Awake()
    {
        base.Awake();
        _dataService = new FileDataService(new JsonSerializer());
        //_cloudDataService = new CloudDataService();

    }

    public void NewGame()
    {
        _gameData = new();
        _gameData.Name = _gameDataSO.Name;
        _gameDataSO.GameData = _gameData;
    }

    public void LoadGame(string gameName)
    {
        _fileGameData = _dataService.Load(gameName);
        if (_fileGameData != null)
        {
            _gameData = _fileGameData;
        }

        if (String.IsNullOrWhiteSpace(_gameData.CurrentLevelName))
        {
            _gameData.CurrentLevelName = "Demo";
        }

        if (_saveDataRTS == null)
        {
            Debug.LogError($"{_saveDataRTS} Not Present. Fix");
            return;
        }

        _gameDataSO.GameData = _gameData;

        for (int i = 0; i < _saveDataRTS.Items.Count; i++)
        {
            _saveDataRTS.Items[i].LoadData(_gameDataSO.GameData);
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
            _saveDataRTS.Items[i].SaveData(_gameDataSO.GameData);
        }

        //_cloudDataService.Save(GameData);
        _gameDataSO.GameData.Name = _gameDataSO.Name;
        _dataService.Save(_gameDataSO.GameData);
    }

    

}
