using UnityEngine;

public abstract class AgentDataSO : SerializableScriptableObject
{
    [SerializeField] protected StatsSO _agentStats;
    [SerializeField] protected MovementDataSO _movementData;
    [SerializeField] protected Sprite _sprite;

    public StatsSO AgentStats => _agentStats;
    public MovementDataSO MovementData => _movementData;
    public Sprite Sprite => _sprite;
}
