using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAgentDataSO", menuName = "Scriptable Objects/Agent/EnemyAgentDataSO")]
public class EnemyAgentDataSO : AgentDataSO
{
    [SerializeField] protected DeathRewardSO _deathReward;

    public DeathRewardSO DeathReward => _deathReward;

}
