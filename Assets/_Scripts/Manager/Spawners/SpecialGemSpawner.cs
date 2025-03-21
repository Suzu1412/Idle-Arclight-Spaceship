using System;
using System.Collections;
using UnityEngine;

public class SpecialGemSpawner : MonoBehaviour, IPausable
{
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private BoolVariableSO _isPaused;
    [SerializeField] private FloatVariableSO _minDelayBetweenSpawns;
    [SerializeField] private FloatVariableSO _maxDelayBetweenSpawns;
    [SerializeField] private SpecialGemConfigSO _gemConfigs;

    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [Header("Void Game Event Binding")]
    [SerializeField] private VoidGameEventBinding OnStartGameEventBinding;
    private Coroutine _spawnGemCoroutine;
    private Action SpawnGemAction;

    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void Awake()
    {
        SpawnGemAction = SpawnGem;
    }

    private void OnEnable()
    {
        OnStartGameEventBinding.Bind(SpawnGemAction, this);
    }

    private void OnDisable()
    {
        if (_spawnGemCoroutine != null) StopCoroutine(_spawnGemCoroutine);

        OnStartGameEventBinding.Unbind(SpawnGemAction, this);
    }

    private void SpawnGem()
    {
        if (this == null) return;  // Prevent executing on destroyed object

        if (_spawnGemCoroutine != null) StopCoroutine(_spawnGemCoroutine);
        _spawnGemCoroutine = StartCoroutine(SpawnGemCoroutine());
    }

    private IEnumerator SpawnGemCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_initialDelay);
        float delayBetweenSpawns = 0f;

        while (true)
        {
            while (delayBetweenSpawns > 0)
            {
                while (_isPaused.Value)
                {
                    yield return null; // Wait until unpaused
                }

                // Increment elapsed time while unpaused
                delayBetweenSpawns -= Time.deltaTime;
                yield return null;
            }

            if (delayBetweenSpawns <= 0f)
            {


                //_randomPattern = UnityEngine.Random.Range(0, _gemPatterns[(int)_currentGemPattern.Value].GemPatternPools.Length);

                //ObjectPooler patternPool = ObjectPoolFactory.Spawn(_gemPatterns[(int)_currentGemPattern.Value].GemPatternPools[_randomPattern]);
                //_newPosition = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));

                //for (int i = 0; i < patternPool.transform.childCount; i++)
                //{
                //    ItemPickUp gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).GetComponent<ItemPickUp>();
                //    gem.SetItem(_gemConfigs[0].Item);

                //    patternPool.transform.position = _newPosition;

                //    gem.RB.position = patternPool.transform.GetChild(i).position;
                //    gem.transform.position = patternPool.transform.GetChild(i).position;
                //}

                delayBetweenSpawns = UnityEngine.Random.Range(_minDelayBetweenSpawns.Value, _maxDelayBetweenSpawns.Value);
                //ObjectPoolFactory.ReturnToPool(patternPool);
            }
        }
    }


    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Pause(bool isPaused)
    {

    }
}
