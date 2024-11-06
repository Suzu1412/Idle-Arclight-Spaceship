using UnityEngine;

public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Managers")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private PlayerManager _playerManager;

    [Header("Bool Event Listener")]
    [SerializeField] private BoolGameEventListener OnSceneGroupLoadedEventListener;

    [Header("Spawners")]
    [SerializeField] private GemSpawner _gemSpawner;

    protected override void Awake()
    {
        base.Awake();
        OnSceneGroupLoadedEventListener.Register(StartGame);
    }

    private void StartGame(bool value)
    {
        BindObjects();
        Initialize();
        LoadGame();
    }

    private void BindObjects()
    {
        _playerManager = PlayerManager.Instance;
        _saveSystem = SaveSystem.Instance;
        _currencyManager = CurrencyManager.Instance;
    }

    private void Initialize()
    {
        _playerManager.SpawnPlayer();
    }

    private void LoadGame()
    {
        _saveSystem.LoadGame();
    }

}
