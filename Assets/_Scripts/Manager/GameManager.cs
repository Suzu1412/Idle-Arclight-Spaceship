using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class GameManager : Singleton<GameManager>, ISaveable
{
    [Header("Persistence")]
    private SaveSystem _saveSystem;
    private GameData _gameData;

    [SerializeField] private GameRulesSO _explorationRules;

    public SerializableGuid Id { get; set; }

    public event UnityAction<GameStateType> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();
        _saveSystem = SaveSystem.Instance;
        PrepareGameData();
    }

    protected void OnEnable()
    {

    }


    public void SaveData(GameData gameData)
    {
    }

    public void LoadData(GameData gameData)
    {
    }

    private void OnEnable()
    {

    }

    private void PrepareGameData()
    {
        _gameData = new();
    }



    private void SaveGame()
    {

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}

