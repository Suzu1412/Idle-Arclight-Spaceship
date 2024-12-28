using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerManager : Singleton<PlayerManager>, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;

    [SerializeField] private GameObject _playerPrefab;
    private GameObject _player;
    [SerializeField] private Vector2 _initialPosition = new(0f, -2f);
    [SerializeField] private PlayerAgentDataSO _defaultPlayerData = default;
    [SerializeField] private ListPlayerAgentDataSO _players;
    private PlayerAgentDataSO _currentPlayerData;
    private Agent _agent;
    private Coroutine _respawnCoroutine;
    private float _respawnTime = 2.5f;

    
    private void OnEnable()
    {
        _saveable.Add(this);
        OnStartGameEventListener.Register(SpawnPlayer);
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnStartGameEventListener.DeRegister(SpawnPlayer);
    }

    public async void LoadData(GameDataSO gameData)
    {
        var players = gameData.Players;
        bool _isPlayerSet = false;

        foreach (var player in players)
        {
            var loadItemOperationHandle = Addressables.LoadAssetAsync<PlayerAgentDataSO>(player.Guid);
            await loadItemOperationHandle.Task;
            if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var playerSO = loadItemOperationHandle.Result;
                playerSO.TotalExp = player.TotalExp;
                playerSO.IsUnlocked = player.IsUnlocked;
                if (player.IsActive)
                {
                    ChangePlayer(playerSO);
                    _isPlayerSet = true;
                }
            }
        }

        if (!_isPlayerSet)
        {
            ChangePlayer(_defaultPlayerData);
        }
    }

    public void SaveData(GameDataSO gameData)
    {
        var players = gameData.Players;
        players.Clear();

        foreach (var playerData in _players.Players)
        {
            players.Add(new PlayerAgentData(playerData.Guid,
            playerData.TotalExp,
            _currentPlayerData == playerData,
            playerData.IsUnlocked
            ));
        }
        gameData.SavePlayers(players);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void SpawnPlayer()
    {
        if (_player == null)
        {
            _player = Instantiate(_playerPrefab);
        }
        _player.SetActive(true);
        _agent = _player.GetComponentInChildren<Agent>();
        _agent.HealthSystem.OnDeath += PlayerDeath;
        SetPosition();
    }

    private void SetPosition()
    {
        _agent.transform.position = _initialPosition;
    }

    private void PlayerDeath()
    {
        _agent.HealthSystem.OnDeath -= PlayerDeath;
        Respawn();
    }

    private void Respawn()
    {
        if (_respawnCoroutine != null) StopCoroutine(_respawnCoroutine);
        _respawnCoroutine = StartCoroutine(RespawnCoroutine());
    }

    public void ChangePlayer(PlayerAgentDataSO playerData)
    {
        if (!playerData.IsUnlocked)
        {
            return;
        }

        if (_agent == null)
        {
            SpawnPlayer();
        }
        _agent.SetPlayerData(playerData);
        _currentPlayerData = playerData;
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_respawnTime);
        SpawnPlayer();
        ChangePlayer(_currentPlayerData);

    }
}
