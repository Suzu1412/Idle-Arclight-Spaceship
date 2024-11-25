using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerManager : Singleton<PlayerManager>, ISaveable
{
    [SerializeField] private GameObject _playerPrefab;
    private GameObject _player;
    [SerializeField] private Vector2 _initialPosition = new(0f, -2f);
    [SerializeField] private PlayerAgentDataSO _defaultPlayerData = default;
    [SerializeField] private ListPlayerAgentDataSO _players;
    private PlayerAgentDataSO _currentPlayerData;
    private Agent _agent;
    private Coroutine _respawnCoroutine;
    private float _respawnTime = 2.5f;

    public async void LoadDataAsync(GameDataSO gameData)
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

    public void SpawnPlayer()
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
