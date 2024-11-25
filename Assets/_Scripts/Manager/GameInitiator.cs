using UnityEngine;

public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Managers")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private PlayerManager _playerManager;

    [Header("Spawners")]
    [SerializeField] private GemSpawner _gemSpawner;

    private void OnEnable()
    {
        StartGame();
    }

    private void StartGame()
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
