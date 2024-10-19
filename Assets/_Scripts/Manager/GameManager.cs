using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class GameManager : Singleton<GameManager>, ISaveable
{
    [Header("Persistence")]
    [SerializeField] private GameDataSO _gameDataSO = default;
    [SerializeField] private GameObjectRuntimeSetSO _saveDataRTS = default;
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
        
    }

    private void Start()
    {
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
        _gameData.Name = _gameDataSO.Name;

        _currentGameState = GameStateType.Init;
        _gameData.CurrentGameState = _currentGameState;
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
            Debug.Log(item);
            item.GetComponent<ISaveable>().LoadData(_gameDataSO.GameData);
        }
    }

    private void SaveGame()
    {
        Debug.Log("intentando guardar");

        foreach (var item in _saveDataRTS.Items)
        {
            Debug.Log(item);
            item.GetComponent<ISaveable>().SaveData(_gameDataSO.GameData);
        }
        Debug.Log("llegando al save system");

        SaveSystem.Instance.SaveGame(_gameDataSO.GameData);
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

