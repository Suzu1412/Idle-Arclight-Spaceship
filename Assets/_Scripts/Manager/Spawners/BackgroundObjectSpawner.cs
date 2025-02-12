using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectSpawner : MonoBehaviour, IPausable
{
    [SerializeField] private BoolVariableSO _isPaused;
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private float _minDelayBetweenSpawns = 30f;
    [SerializeField] private float _maxDelayBetweenSpawns = 30f;
    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private BackgroundObjectConfigSO _objectConfig;


    [Header("Void Game Event Binding")]
    [SerializeField] private VoidGameEventBinding OnStartGameEventBinding;
    private Coroutine _spawnObjectCoroutine;
    private Action SpawnObjectAction;

    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void Awake()
    {
        SpawnObjectAction = SpawnObject;
    }

    private void OnEnable()
    {
        OnStartGameEventBinding.Bind(SpawnObjectAction, this);
        _pausable.Add(this);
    }

    private void OnDisable()
    {
        OnStartGameEventBinding.Unbind(SpawnObjectAction, this);
        _pausable.Remove(this);
    }

    private void SpawnObject()
    {
        if (_spawnObjectCoroutine != null) StopCoroutine(_spawnObjectCoroutine);
        _spawnObjectCoroutine = StartCoroutine(SpawnObjectCoroutine());
    }


    private IEnumerator SpawnObjectCoroutine()
    {
        float delayBetweenSpawns = UnityEngine.Random.Range(_minDelayBetweenSpawns, _maxDelayBetweenSpawns);
        while (true)
        {
            if (!_isPaused.Value)
            {
                delayBetweenSpawns -= Time.deltaTime;
            }

            yield return null;

            if (delayBetweenSpawns <= 0f)
            {
                BackgroundObject backgroundObject = ObjectPoolFactory.Spawn(_objectConfig.PoolSettings).GetComponent<BackgroundObject>();
                int selectedSprite = UnityEngine.Random.Range(0, _objectConfig.SpriteList.Count);
                backgroundObject.SetSprite(_objectConfig.SpriteList[selectedSprite]);
                int selectedConfiguration = UnityEngine.Random.Range(0, _objectConfig.ConfigurationList.Count);
                backgroundObject.SetMoveSpeed(_objectConfig.ConfigurationList[selectedConfiguration].MoveSpeed);
                backgroundObject.SetScale(_objectConfig.ConfigurationList[selectedConfiguration].Scale);
                backgroundObject.SetOrderInLayer(_objectConfig.ConfigurationList[selectedConfiguration].SortingOrder);
                var position = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));
                backgroundObject.RB.position = position;
                backgroundObject.transform.position = position;

                delayBetweenSpawns = UnityEngine.Random.Range(_minDelayBetweenSpawns, _maxDelayBetweenSpawns);
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
