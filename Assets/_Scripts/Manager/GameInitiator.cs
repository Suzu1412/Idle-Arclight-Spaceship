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

    private void Start()
    {
        BindObjects();
        Initialize();
        LoadGame();
    }

    private void BindObjects()
    {
        _playerManager = Instantiate(_playerManager);
        _saveSystem = Instantiate(_saveSystem);
        _currencyManager = Instantiate(_currencyManager);


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
