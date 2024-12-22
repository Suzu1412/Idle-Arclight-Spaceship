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

    [Header("Void Game Event Listener")]
    [SerializeField] private VoidGameEventListener OnStartGameEventListener;
    private Coroutine _spawnGemCoroutine;

    private Vector3 _newPosition;

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

            _newPosition = _placementStrategy.SetPosition(new Vector3(0f, 9f, 0f));

            if (gem.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.position = _newPosition;
            }
            gem.transform.position = _newPosition;


            _delayBetweenSpawns = Random.Range(_minDelayBetweenSpawns.Value, _maxDelayBetweenSpawns.Value);
            yield return Helpers.GetWaitForSeconds(_delayBetweenSpawns);

        }

    }
}
