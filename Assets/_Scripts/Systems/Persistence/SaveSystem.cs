using System;
using UnityEngine;

public class SaveSystem : Singleton<SaveSystem>
{
    IDataService _dataService;
    private bool _appIsPaused = false;
    private bool _canSave;
    private bool _isGameDataReady;

    [Header("Void Game Event Binding")]
    [SerializeField] private VoidGameEventBinding OnStartGameEventBinding;
    [SerializeField] private VoidGameEventBinding OnLoadLastSceneEventBinding;
    [SerializeField] private VoidGameEventBinding OnDeleteSaveDataEventBinding;

    [Header("Int Game Event")]
    [SerializeField] private IntGameEvent OnChangeSceneEvent;

    [Header("Bool Game Event Listener")]

    [Header("Persistence")]
    [SerializeField] private GameDataSO _gameDataSO = default;
    [SerializeField] private SaveableRunTimeSetSO _saveDataRTS = default;

    // Actions
    private Action LoadGameAction;
    private Action LoadLastSceneAction;
    private Action DeleteAllFileAction;

    protected override void Awake()
    {
        base.Awake();
        PrepareGameData();

        LoadGameAction = LoadGame;
        LoadLastSceneAction = LoadLastScene;
        DeleteAllFileAction = DeleteAllFileData;
    }

    private void OnEnable()
    {
        _canSave = false;
        OnStartGameEventBinding.Bind(LoadGameAction, this);
        OnLoadLastSceneEventBinding.Bind(LoadLastSceneAction, this);
        OnDeleteSaveDataEventBinding.Bind(DeleteAllFileAction, this);
    }

    private void OnDisable()
    {
        OnStartGameEventBinding.Unbind(LoadGameAction, this);
        OnLoadLastSceneEventBinding.Unbind(LoadLastSceneAction, this);
        OnDeleteSaveDataEventBinding.Unbind(DeleteAllFileAction, this);

    }

    private async void LoadLastScene()
    {
        _gameDataSO ??= await LoadDataFromFile(_gameDataSO);

        OnChangeSceneEvent.RaiseEvent(0, this);
    }

    private async void LoadGame()
    {
        // If game data is not found, initialize it with default values
        if (_gameDataSO == null)
        {
            _gameDataSO = ScriptableObject.CreateInstance<GameDataSO>();
            _gameDataSO.Initialize();
        }


        _gameDataSO = await LoadDataFromFile(_gameDataSO);

        foreach (var item in _saveDataRTS.Items)
        {
            item.LoadData(_gameDataSO);
        }
        _canSave = true;
    }

    private void SaveGame()
    {
        foreach (var item in _saveDataRTS.Items)
        {
            item.SaveData(_gameDataSO);
        }

        SaveDataToFile(_gameDataSO);
    }

    private async Awaitable<GameDataSO> LoadDataFromFile(GameDataSO gameData)
    {
        _dataService = new FileDataService(new JsonSerializer());

        _gameDataSO = await _dataService.Load(gameData);

        return _gameDataSO;
    }

    private void SaveDataToFile(GameDataSO gameDataSO)
    {
        //_cloudDataService.Save(gameData);
        _dataService.Save(gameDataSO);
    }

    private void DeleteFileData(GameDataSO gameDataSO)
    {
        _dataService.Delete(gameDataSO.name);
    }

    private void DeleteAllFileData()
    {
        _gameDataSO.Initialize();
        DeleteFileData(_gameDataSO);
        LoadLastScene();
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
        if (!_canSave) return;
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
        if (!_canSave) return;
        _appIsPaused = !hasFocus;
        if (!hasFocus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        if (!_canSave) return;
        SaveGame();
    }

}
