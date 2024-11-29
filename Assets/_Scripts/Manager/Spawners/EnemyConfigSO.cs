using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigSO", menuName = "Scriptable Objects/Spawner/EnemyConfigSO")]
public class EnemyConfigSO : ScriptableObject
{
    [SerializeField] private ObjectPoolSettingsSO _poolSettings;
    [SerializeField] private EnemyAgentDataSO _agentData;

    public ObjectPoolSettingsSO PoolSettings => _poolSettings;
    public EnemyAgentDataSO AgentData => _agentData;
}
