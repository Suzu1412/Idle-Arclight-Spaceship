using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleWaveSO", menuName = "Scriptable Objects/Spawner/Waves/ObstacleWaveSO")]
public class ObstacleWaveSO : WaveSO
{
    [SerializeField] private ObjectPoolSettingsSO _obstaclePrefabPool;
    [SerializeField] private List<ObstacleSpawn> _spawnObstacles;
    private int _currentSpawnIndex;
    private Vector3 _position;

    [SerializeField] private float minPositionX = -1.8f;
    [SerializeField] private float maxPositionX = 1.8f;
    [SerializeField] private float minPositionY = 6f;
    [SerializeField] private float maxPositionY = 9f;


    protected override void OnSpawn()
    {
        var agent = ObjectPoolFactory.Spawn(_obstaclePrefabPool).GetComponentInChildren<Agent>();

        _spawnObstacles[_currentSpawnIndex].ObstacleData.InitializeAgent(agent, GetPosition());

        if (_currentSpawnIndex + 1 == _spawnObstacles.Count)
        {
            WaveEnded = true;
        }

        _currentSpawnIndex = _spawnObstacles.MoveForward(_currentSpawnIndex); // Will return to 0 if its the last one
    }

    protected override Vector3 GetPosition()
    {
        var spawnObstacle = _spawnObstacles[_currentSpawnIndex];

        float x = Random.Range(minPositionX, maxPositionX);
        float y = Random.Range(minPositionY, maxPositionY);
        _position = new(x, y, 0);
        return _position;

    }
}

[System.Serializable]
public class ObstacleSpawn
{
    public ObstacleDataSO ObstacleData;
}