using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private WaitUntilSO _waitUntil;
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private FloatVariableSO _minDelayBetweenSpawns;
    [SerializeField] private FloatVariableSO _maxDelayBetweenSpawns;
    [SerializeField] private BoolVariableSO _isPaused;

    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private List<GemConfigSO> _gemConfigs;
    [SerializeField] private GemPatternSO[] _gemPatterns;
    [SerializeField] private FloatVariableSO _currentGemPattern;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnGemCoroutine;

    private Vector3 _newPosition;
    private int _randomPattern;

    public WaitUntilSO WaitUntil { get => _waitUntil; set => _waitUntil = value; }
    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void OnEnable()
    {
        OnStartGameEventListener.Register(SpawnGem);
        _pausable.Add(this);
    }

    private void OnDisable()
    {
        OnStartGameEventListener.DeRegister(SpawnGem);
        _pausable.Remove(this);

    }

    private void SpawnGem()
    {
        if (_spawnGemCoroutine != null) StopCoroutine(_spawnGemCoroutine);
        _spawnGemCoroutine = StartCoroutine(SpawnGemCoroutine());
    }


    private IEnumerator SpawnGemCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_initialDelay);
        float delayBetweenSpawns = 0f;

        while (true)
        {
            yield return _waitUntil;
            delayBetweenSpawns -= Time.deltaTime;
            yield return null;

            if (delayBetweenSpawns <= 0f)
            {
                _randomPattern = Random.Range(0, _gemPatterns[(int)_currentGemPattern.Value].GemPatternPools.Length);

                ObjectPooler patternPool = ObjectPoolFactory.Spawn(_gemPatterns[(int)_currentGemPattern.Value].GemPatternPools[_randomPattern]);
                _newPosition = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));

                for (int i = 0; i < patternPool.transform.childCount; i++)
                {
                    ItemPickUp gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).GetComponent<ItemPickUp>();
                    gem.SetItem(_gemConfigs[0].Item);

                    patternPool.transform.position = _newPosition;

                    gem.RB.position = patternPool.transform.GetChild(i).position;
                    gem.transform.position = patternPool.transform.GetChild(i).position;
                }

                delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns.Value, _maxDelayBetweenSpawns.Value);
                ObjectPoolFactory.ReturnToPool(patternPool);
            }
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
