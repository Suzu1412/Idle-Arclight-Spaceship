using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class GameManager : Singleton<GameManager>, ISaveable
{
    [Header("Persistence")]
    [SerializeField] private GameDataSO _gameDataSO = default;
    [SerializeField] private GameObjectRuntimeSetSO _saveDataRTS = default;
    [SerializeField] private TextMeshProUGUI _text;
    private SaveSystem _saveSystem;
    private GameData _gameData;
    private bool _hasLoadedGame = false;
    private bool _appIsPaused = false;

    private GameStateType _currentGameState;

    [SerializeField] private GameRulesSO _explorationRules;

    public SerializableGuid Id { get; set; }

    public event UnityAction<GameStateType> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();
        _saveSystem = SaveSystem.Instance;

    }

    private void Start()
    {
        _hasLoadedGame = false;
        PrepareGameData();

        LoadGame();
    }

    #region Save System
    public void SaveData(GameData gameData)
    {
        gameData.CurrentGameState = _currentGameState;
    }

    public void LoadData(GameData gameData)
    {
        _currentGameState = gameData.CurrentGameState;
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

    private void NewGame()
    {
        _gameData = new();
        _gameData.NewGame(_gameDataSO.Name);
    }

    private async void LoadGame()
    {
        _gameData = await SaveSystem.Instance.LoadGame(_gameDataSO.Name);

        if (_gameData == null)
        {
            NewGame();
        }

        _gameDataSO.GameData = _gameData;

        foreach (var item in _saveDataRTS.Items)
        {
            item.GetComponent<ISaveable>().LoadData(_gameDataSO.GameData);
        }

        _hasLoadedGame = true;
    }

    private void SaveGame()
    {
        if (!_hasLoadedGame)
        {
            return;
        }

        foreach (var item in _saveDataRTS.Items)
        {
            item.GetComponent<ISaveable>().SaveData(_gameDataSO.GameData);
        }

        SaveSystem.Instance.SaveGame(_gameDataSO.GameData);
    }
    #endregion

    private void HandleState(GameStateType currentState)
    {
        _currentGameState = currentState;

        OnStateChanged?.Invoke(_currentGameState);
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
    /// Called when the gamme loses or gains focus. 
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

