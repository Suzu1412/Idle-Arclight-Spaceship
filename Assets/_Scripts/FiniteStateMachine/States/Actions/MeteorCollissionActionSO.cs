using UnityEngine;

[CreateAssetMenu(fileName = "MeteorCollissionActionSO", menuName = "Scriptable Objects/FSM/Action/MeteorCollissionActionSO")]
public class MeteorCollissionActionSO : ActionSO
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;
    [SerializeField] private SoundDataSO _impactSound;
    [SerializeField] private Color _color = Color.green;

    public override void DrawGizmos(FiniteStateMachine fsm)
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(fsm.transform.position, fsm.Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>());
    }

    public override void Execute(FiniteStateMachine fsm)
    {
        foreach (var player in _playerRTS.Items)
        {
            if (fsm.transform.position.IsWithinRange(player.transform.position, fsm.Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>()))
            {
                if (player.TryGetComponent<IHealthSystem>(out var damageable))
                {
                    if (damageable.IsInvulnerable) return;
                    damageable.Damage((int)fsm.Agent.StatsSystem.GetStatValue<AttackStatSO>());
                    _impactSound.PlayEvent();
                }
            }
        }
    }
}
