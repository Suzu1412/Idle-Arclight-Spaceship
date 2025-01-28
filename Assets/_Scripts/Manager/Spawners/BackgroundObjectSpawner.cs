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


    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnObjectCoroutine;

    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void OnEnable()
    {
        OnStartGameEventListener.Register(SpawnObject);
        _pausable.Add(this);
    }

    private void OnDisable()
    {
        OnStartGameEventListener.DeRegister(SpawnObject);
        _pausable.Remove(this);
    }

    private void SpawnObject()
    {
        if (_spawnObjectCoroutine != null) StopCoroutine(_spawnObjectCoroutine);
        _spawnObjectCoroutine = StartCoroutine(SpawnObjectCoroutine());
    }


    private IEnumerator SpawnObjectCoroutine()
    {
        float delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns, _maxDelayBetweenSpawns);
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
                int selectedSprite = Random.Range(0, _objectConfig.SpriteList.Count);
                backgroundObject.SetSprite(_objectConfig.SpriteList[selectedSprite]);
                int selectedConfiguration = Random.Range(0, _objectConfig.ConfigurationList.Count);
                backgroundObject.SetMoveSpeed(_objectConfig.ConfigurationList[selectedConfiguration].MoveSpeed);
                backgroundObject.SetScale(_objectConfig.ConfigurationList[selectedConfiguration].Scale);
                backgroundObject.SetOrderInLayer(_objectConfig.ConfigurationList[selectedConfiguration].SortingOrder);
                var position = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));
                backgroundObject.RB.position = position;
                backgroundObject.transform.position = position;

                delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns, _maxDelayBetweenSpawns);
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
