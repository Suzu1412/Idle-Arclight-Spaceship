using UnityEngine;

public abstract class AgentDataSO : SerializableScriptableObject
{
    [SerializeField] protected StatsSO _agentStats;
    [SerializeField] protected MovementDataSO _movementData;
    [SerializeField] protected StateListSO _states; 

    public virtual void InitializeAgent(Agent agent, Vector3 position)
    {
        agent.MoveBehaviour.SetMoveData(_movementData);
        agent.StatsSystem.SetStatsData(_agentStats);
        agent.HealthSystem.Initialize((int)agent.StatsSystem.GetStat<HealthStatSO>().Value);
        agent.FSM.SetStates(_states);
        agent.transform.position = position;
        agent.MoveBehaviour.RB.position = position;
    }

}
