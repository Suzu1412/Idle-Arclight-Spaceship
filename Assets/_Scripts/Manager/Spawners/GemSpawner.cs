using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private float _minDelayBetweenSpawns = 0.25f;
    [SerializeField] private float _maxDelayBetweenSpawns = 1f;
    private float _delayBetweenSpawns;
    [SerializeField] private BasePlacementStrategySO _placementStrategy;
    [SerializeField] private List<GemConfigSO> _gemConfigs;

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnGemCoroutine;

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
            GameObject gem = ObjectPoolFactory.Spawn(_gemConfigs[0].PoolSettings).gameObject;
            gem.GetComponent<ItemPickUp>().SetItem(_gemConfigs[0].Item);
            //gem.GetComponentInChildren<SpriteRenderer>().material = _gemConfigs[0].Material;
            gem.GetComponent<TransformMover>().SetMoveSpeed(_gemConfigs[0].moveSpeed);

            gem.transform.position = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));

            _delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns, _maxDelayBetweenSpawns);
            yield return Helpers.GetWaitForSeconds(_delayBetweenSpawns);
        }

    }
}
