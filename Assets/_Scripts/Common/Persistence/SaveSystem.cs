using System;
using UnityEngine;

public class SaveSystem : PersistentSingleton<SaveSystem>
{
    [SerializeField] private GameData GameData;
    private GameData _fileGameData;
    private GameData _cloudGameData;
    [SerializeField] private SaveableRunTimeSetSO _saveDataRTS = default;
    private const string _saveDataPath = "SaveSystem/";

    IDataService _dataService;
    ICloudService _cloudDataService;

    protected override void Awake()
    {
        base.Awake();

        _dataService = new FileDataService(new JsonSerializer());
        _cloudDataService = new CloudDataService();
    }

    private void Start()
    {
        LoadGame(GameData.Name);
    }

    public async void LoadGame(string gameName)
    {
        _cloudGameData = await _cloudDataService.Load(gameName);
        _fileGameData = _dataService.Load(gameName);

        if (_cloudGameData == null && _fileGameData == null)
        {
            return;
        }

        if (_cloudGameData.LastSavedTime == _fileGameData.LastSavedTime)
        {
            GameData = _cloudGameData;
        }
        else
        {
            if (_cloudGameData.LastSavedTime > _fileGameData.LastSavedTime)
            {
                GameData = _cloudGameData;
            }
            else
            {
                GameData = _fileGameData;
            }
        }

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

        GameData.LastSavedTime = Time.time;
        _cloudDataService.Save(GameData);
        _dataService.Save(GameData);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
