using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThematicWaveSO", menuName = "Scriptable Objects/Spawner/Waves/ThematicWaveSO")]
public class ThematicWaveSO : WaveSO
{
    [SerializeField] private ObjectPoolSettingsSO _enemyPrefabPool;
    [SerializeField] private List<ThematicEnemySpawn> _spawnEnemies;
    private int _currentSpawnIndex;

    protected override Vector3 GetPosition()
    {
        return _spawnEnemies[_currentSpawnIndex].Position;

    }

    protected override void OnSpawn()
    {
        var agent = ObjectPoolFactory.Spawn(_enemyPrefabPool).GetComponentInChildren<Agent>();

        _spawnEnemies[_currentSpawnIndex].EnemyData.InitializeAgent(agent, GetPosition());

        if (_currentSpawnIndex + 1 == _spawnEnemies.Count)
        {
            WaveEnded = true;
        }

        _currentSpawnIndex = _spawnEnemies.MoveForward(_currentSpawnIndex); // Will return to 0 if its the last one
    }
}

[System.Serializable]
public class ThematicEnemySpawn
{
    public EnemyAgentDataSO EnemyData;
    public Vector3 Position;
}
