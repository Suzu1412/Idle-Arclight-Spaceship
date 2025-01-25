using UnityEngine;

public abstract class WaveSO : ScriptableObject
{
    [SerializeField] protected int _baseEnemiesPerWave = 1;
    [SerializeField] protected int _wavesToIncreaseAmount = 5;
    [SerializeField] protected int _maxAmountOfEnemies = 20;

    [Header("Time between spawns")]
    [SerializeField] [ReadOnly] protected float _elapsedTime = 0f;
    [SerializeField] [ReadOnly] protected float _spawnTimer = 0f;
    [SerializeField] protected float _spawnInterval = 0f;

    public bool WaveEnded = false;

    public virtual void Initialize()
    {
        _elapsedTime = 0f;
        _spawnTimer = 0f;
        WaveEnded = false;
    }

    public void UpdateWave(float deltaTime)
    {
        _elapsedTime += deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            OnSpawn();
        }

    }

    protected abstract void OnSpawn();
    protected abstract Vector3 GetPosition();
}
