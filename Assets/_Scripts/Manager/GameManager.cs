using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class GameManager : Singleton<GameManager>, ISaveable
{
    [Header("Persistence")]
    [SerializeField] private GameDataSO _gameDataSO = default;
    [SerializeField] private SaveableRunTimeSetSO _saveDataRTS = default;
    private SaveSystem _saveSystem;
    private GameData _gameData;

    private GameStateType _currentGameState;

    [SerializeField] private GameRulesSO _explorationRules;

    public SerializableGuid Id { get; set; }

    public event UnityAction<GameStateType> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();
        _saveSystem = SaveSystem.Instance;
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
        _gameDataSO.GameData = new();

        _gameData = new();
        _gameData.Name = _gameDataSO.Name;

        _currentGameState = GameStateType.Init;
    }

    private async void LoadGame()
    {
        _gameData = await _saveSystem.LoadGame(_gameDataSO.Name);

        if (_gameData == null)
        {
            NewGame();
        }

        _gameDataSO.GameData = _gameData;

        foreach (var item in _saveDataRTS.Items)
        {
            item.LoadData(_gameDataSO.GameData);
        }
    }

    private void SaveGame()
    {
        foreach (var item in _saveDataRTS.Items)
        {
            item.SaveData(_gameDataSO.GameData);
        }

        _saveSystem.SaveGame(_gameDataSO.GameData);
    }
    #endregion

    private void HandleState(GameStateType currentState)
    {
        _currentGameState = currentState;

        OnStateChanged?.Invoke(_currentGameState);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}

