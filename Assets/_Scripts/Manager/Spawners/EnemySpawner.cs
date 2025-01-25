using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IPausable
{
    [SerializeField] private BoolVariableSO _isPaused;
    [SerializeField] private WaitUntilSO _waitUntil;
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private float _initialDelay = 60f;
    [SerializeField] private float _delayBetweenWaves = 10f;
    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private int _currentWave = -1;
    [SerializeField] private SpawnWaveType _waveType;

    [SerializeField] private List<WaveSO> _allWaves;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnEnemyCoroutine;

    public WaitUntilSO WaitUntil { get => _waitUntil; set => _waitUntil = value; }
    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

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
            yield return _waitUntil.WaitUntil;
            delayBetweenSpawns -= Time.deltaTime;
            yield return null;

            if (delayBetweenSpawns <= 0f)
            {
                _allWaves[0].UpdateWave(Time.deltaTime);

                if (_allWaves[0].WaveEnded)
                {
                    _allWaves[0].Initialize();
                    // AdvanceWave();
                    delayBetweenSpawns = _delayBetweenWaves;
                }
            }
        }


            //    AdvanceWave();

            //    float delayBetweenSpawns = 0f;

            //    while (true)
            //    {
            //        if (!_isPaused.Value)
            //        {
            //            delayBetweenSpawns -= Time.deltaTime;
            //        }

            //        yield return null;

            //        if (delayBetweenSpawns <= 0f)
            //        {
            //            switch (_waveType)
            //            {
            //                case SpawnWaveType.Enemies:

            //                    break;

            //                case SpawnWaveType.Obstacle:
            //                    var obstacle = _obstacleWave;

            //                    break;

            //                case SpawnWaveType.Boss:

            //                    break;

            //                default:
            //                    Debug.LogError("Enemy Wave should not be here, there's an error");
            //                    break;
            //            }


            //            var randomEnemy = Random.Range(0, _enemyConfigs.Count);
            //            GameObject enemy = ObjectPoolFactory.Spawn(_enemyPrefabPool).gameObject;
            //            //enemy.GetComponentInChildren<Agent>().SetEnemyData(_enemyConfigs[randomEnemy].AgentData);
            //            var position = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));
            //            //enemy.GetComponentInChildren<MovementBehaviour>().RB.position = position;
            //            //enemy.transform.GetChild(0).position = position;
            //            //enemy.transform.GetChild(1).position = position;

            //            delayBetweenSpawns = _delayBetweenSpawns;

            //            AdvanceWave();
            //        }
            //    }

    }

    private void AdvanceWave()
    {
        _currentWave++;
        //SetWaveType(_currentWave);
    }
    private void SetWaveType(int currentWave)
    {
        int cycle = (currentWave - 1) % 10; // Zero-based position in cycle (0-9)
        int obstacleCount = (currentWave % 20 < 10) ? 2 : 3; // Alternate obstacle count every 10 waves

        if (cycle < obstacleCount)
        {
            _waveType = SpawnWaveType.Obstacle;
        }
        //else if (cycle == 4) // special kind of wave
        //{
            // 
        //}
        else if (cycle == 9)
        {
            _waveType = SpawnWaveType.Boss;
        }
        else
        {
            _waveType = SpawnWaveType.Enemies;
        }

    }


    public void Pause(bool isPaused)
    {
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}


[System.Serializable]
public class WaveGenerator
{
    [Header("Wave Settings")]
    [SerializeField] private int _totalWaves = 10;
    [SerializeField] private ObstacleWaveSO _obstacleWaveData;
    [SerializeField] private WaveSO _bossWaveData;



    private List<WaveSO> _generatedWaves;

    public List<WaveSO> GenerateWaves()
    {
        _generatedWaves = new();

        for (int i = 0; i < _totalWaves; i++)
        {
            if (i == 0 || i == 1)
            {

            }


        }



        return _generatedWaves;
    }

}