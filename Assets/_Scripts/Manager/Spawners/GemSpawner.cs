using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private FloatVariableSO _minDelayBetweenSpawns;
    [SerializeField] private FloatVariableSO _maxDelayBetweenSpawns;

    private float _delayBetweenSpawns;
    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private List<GemConfigSO> _gemConfigs;
    [SerializeField] private GemPatternSO[] _gemPatterns;
    [SerializeField] private FloatVariableSO _currentGemPattern;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnGemCoroutine;

    private Vector3 _newPosition;
    private int _randomPattern;

    private void OnEnable()
    {
        OnStartGameEventListener.Register(SpawnGem);
    }

    private void OnDisable()
    {
        OnStartGameEventListener.DeRegister(SpawnGem);
    }

    private void SpawnGem()
    {
        if (_spawnGemCoroutine != null) StopCoroutine(_spawnGemCoroutine);
        _spawnGemCoroutine = StartCoroutine(SpawnGemCoroutine());
    }


    private IEnumerator SpawnGemCoroutine()
    {
        yield return Helpers.GetWaitForSeconds(_initialDelay);

        while (true)
        {
            _randomPattern = Random.Range(0, _gemPatterns[(int)_currentGemPattern.Value].GemPatternPools.Length);

            GameObject pattern = ObjectPoolFactory.Spawn(_gemPatterns[(int)_currentGemPattern.Value].GemPatternPools[_randomPattern]).gameObject;
            _newPosition = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));

            for(int i= 0; i < pattern.transform.childCount; i++)
            {
                ItemPickUp gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).GetComponent<ItemPickUp>();
                gem.SetItem(_gemConfigs[0].Item);

                pattern.transform.position = _newPosition;

                gem.RB.position = pattern.transform.GetChild(i).position; 
                gem.transform.position = pattern.transform.GetChild(i).position; 
            }

            _delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns.Value, _maxDelayBetweenSpawns.Value);
            yield return Helpers.GetWaitForSeconds(_delayBetweenSpawns);

        }

    }
}
