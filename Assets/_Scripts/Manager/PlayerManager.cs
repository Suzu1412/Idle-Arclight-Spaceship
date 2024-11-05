using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerManager : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector2 _initialPosition = new(0f, -2f);
    [SerializeField] private PlayerAgentDataSO _defaultPlayerConfig = default;
    private PlayerConfig _playerConfig;
    private Agent _agent;
    private PlayerAgentData _currentPlayerData;
    private Coroutine _respawnCoroutine;
    private float _respawnTime = 2.5f;

    public void LoadData(GameData gameData)
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

        //_defaultPlayerConfig.SetCurrentHealth(_currentPlayerData.CurrentHealth);
        //_defaultPlayerConfig.SetTotalExp(_currentPlayerData.TotalExp);
        //_defaultPlayerConfig.SetCurrentLevel(_currentPlayerData.CurrentLevel);
    }

    public void SaveData(GameData gameData)
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
        _playerPrefab = Instantiate(_playerPrefab);
        _playerPrefab.SetActive(true);
        _playerConfig = _playerPrefab.GetComponent<PlayerConfig>();
        _agent = _playerConfig.GetComponentInChildren<Agent>();
 
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

    private void Respawn()
    {
        if (_respawnCoroutine != null) StopCoroutine(_respawnCoroutine);
        _respawnCoroutine = StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_respawnTime);

    }
}
