using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private float _initialDelay = 60f;
    [SerializeField] private float _delayBetweenSpawns = 30f;
    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private List<EnemyConfigSO> _enemyConfigs;
    private bool _isPaused;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnEnemyCoroutine;

    private void OnEnable()
    {
        OnStartGameEventListener.Register(SpawnEnemy);
        _pausable.Add(this);
    }

    private void OnDisable()
    {
        OnStartGameEventListener.DeRegister(SpawnEnemy);
        _pausable.Remove(this);
    }

    private void SpawnEnemy()
    {
        if (_spawnEnemyCoroutine != null) StopCoroutine(_spawnEnemyCoroutine);
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }


    private IEnumerator SpawnEnemyCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_initialDelay);
        float delayBetweenSpawns = 0f;

        while (true)
        {
            if (!_isPaused)
            {
                delayBetweenSpawns -= Time.deltaTime;
            }

            yield return null;

            if (delayBetweenSpawns <= 0f)
            {
                GameObject enemy = ObjectPoolFactory.Spawn(_enemyConfigs[0].PoolSettings).gameObject;
                enemy.GetComponentInChildren<Agent>().SetEnemyData(_enemyConfigs[0].AgentData);
                var position = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));
                enemy.GetComponentInChildren<MovementBehaviour>().RB.position = position;
                enemy.transform.GetChild(0).position = position;
                enemy.transform.GetChild(1).position = position;

                delayBetweenSpawns = _delayBetweenSpawns;
            }
        }

    }

    public void Pause(bool isPaused)
    {
        _isPaused = isPaused;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
