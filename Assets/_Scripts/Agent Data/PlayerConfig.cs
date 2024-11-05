using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    private PlayerAgentDataSO _agentData;
    private MovementBehaviour _movement;
    private IAgent _agent;
    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();
    internal MovementBehaviour Movement => _movement != null ? _movement : _movement = GetComponentInChildren<MovementBehaviour>();

    public void SetPlayerAgentData(PlayerAgentDataSO agentData)
    {
        _agentData = agentData;

        Agent.HealthSystem.Initialize(_agentData.CurrentHealth);
        Movement.SetMovementData(_agentData.MovementData);
    }
}
