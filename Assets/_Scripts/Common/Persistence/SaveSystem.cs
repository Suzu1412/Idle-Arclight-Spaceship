using System;
using UnityEngine;

public class SaveSystem : Singleton<SaveSystem>
{
    private GameData _gameData;
    private GameData _fileGameData;
    IDataService _dataService;
    private bool _appIsPaused = false;

    [Header("Persistence")]
    [SerializeField] private GameDataSO _gameDataSO = default;
    [SerializeField] private GameObjectRuntimeSetSO _saveDataRTS = default;

    protected override void Awake()
    {
        base.Awake();
        PrepareGameData();
    }

    public async void LoadGame()
    {
        _gameData = await LoadDataFromFile(_gameDataSO.Name);

        if (_gameData == null)
        {
            NewGame();
        }

        _gameDataSO.GameData = _gameData;

        foreach (var item in _saveDataRTS.Items)
        {
            item.GetComponent<ISaveable>().LoadDataAsync(_gameDataSO);
        }
    }

    public void SaveGame()
    {
        foreach (var item in _saveDataRTS.Items)
        {
            item.GetComponent<ISaveable>().SaveData(_gameDataSO);
        }

        SaveDataToFile(_gameDataSO.GameData);
    }


    private void NewGame()
    {
        _gameData = new();
        _gameData.NewGame(_gameDataSO.Name);
    }

    private async Awaitable<GameData> LoadDataFromFile(string gameName)
    {
        _dataService = new FileDataService(new JsonSerializer());

        _fileGameData = await _dataService.Load(gameName);

        return _fileGameData;
        //SceneManager.LoadScene(gameData.CurrentLevelName);
    }

    private void SaveDataToFile(GameData gameData)
    {
        //_cloudDataService.Save(gameData);
        _dataService.Save(gameData);
    }

    private void PrepareGameData()
    {
        if (_gameDataSO == null)
        {
            Debug.LogError($"{_gameDataSO} Not Present. Fix");
            return;
        }

        if (_saveDataRTS == null)
        {
            Debug.LogError($"{_saveDataRTS} Not Present. Fix");
            return;
        }
    }

    /// <summary>
    /// Called when the application pauses.
    /// </summary>
    /// <param name="pause"><c>true</c> if the application is paused, else <c>false</c>.</param>
    private void OnApplicationPause(bool pause)
    {
        _appIsPaused = pause;
        if (pause)
            SaveGame();
    }

    /// <summary>
    /// Called when the game loses or gains focus. 
    /// </summary>
    /// <param name="hasFocus"><c>true</c> if the gameobject has focus, else <c>false</c>.</param>
    /// <remarks>
    /// On Windows Store Apps and Windows Phone 8.1 there's no application quit event,
    /// consider using OnApplicationFocus event when hasFocus equals false.
    /// </remarks>
    private void OnApplicationFocus(bool hasFocus)
    {
        _appIsPaused = !hasFocus;
        if (!hasFocus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
