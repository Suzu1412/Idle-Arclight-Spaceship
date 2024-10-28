using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    [SerializeField] private EnemyAgentDataSO _config;
    private IAgent _agent;
    private DeathReward _deathReward;
    private MovementBehaviour _movement;

    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();
    internal MovementBehaviour Movement => _movement != null ? _movement : _movement = GetComponentInChildren<MovementBehaviour>();
    internal DeathReward DeathReward => _deathReward != null ? _deathReward : _deathReward = GetComponentInChildren<DeathReward>();

    private void OnEnable()
    {
        Configure();
    }

    public void Configure()
    {
        Agent.HealthSystem.Initialize(_config.CurrentHealth);
        Movement.SetMovementData(_config.MovementData);
        DeathReward.SetReward(_config.DeathReward);
    }
}
