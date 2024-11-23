using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerManager : Singleton<PlayerManager>, ISaveable
{
    [SerializeField] private GameObject _playerPrefab;
    private GameObject _player;
    [SerializeField] private Vector2 _initialPosition = new(0f, -2f);
    [SerializeField] private PlayerAgentDataSO _defaultPlayerData = default;
    [SerializeField] private ListPlayerAgentDataSO _players;
    private Agent _agent;
    private Coroutine _respawnCoroutine;
    private float _respawnTime = 2.5f;

    public void LoadData(GameDataSO gameData)
    {
        if (gameData.CurrentPlayerData != null)
        {
            _currentPlayerData = gameData.CurrentPlayerData;
        }
        else
        {
            _currentPlayerData = gameData.PlayerAgentDatas.Load(_defaultPlayerConfig.Guid);
        }
        SetPlayerData();
    }

    public void SaveData(GameDataSO gameData)
    {
        var agentData = new PlayerAgentData(
            _currentPlayerData.Guid,
            _agent.HealthSystem.GetCurrentHealth(),
            _agent.LevelSystem.GetTotalExp(),
            _agent.LevelSystem.GetCurrentLevel()
        );
        gameData.CurrentPlayerData = agentData;
        gameData.PlayerAgentDatas.Save(agentData);
    }

    public void SpawnPlayer()
    {
        if (_player == null)
        {
            _player = Instantiate(_playerPrefab);
        }
        _player.SetActive(true);
        _playerConfig = _player.GetComponent<PlayerConfig>();
        _agent = _playerConfig.GetComponentInChildren<Agent>();
        _agent.HealthSystem.OnDeath += PlayerDeath;

        SetPosition();
    }

    private void SetPosition()
    {
        _agent.transform.position = _initialPosition;
    }

    private void SetPlayerData()
    {
        _playerConfig.SetPlayerAgentData(_defaultPlayerConfig);
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

    private IEnumerator RespawnCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_respawnTime);
        SpawnPlayer();
        SetPlayerData();
    }
}
