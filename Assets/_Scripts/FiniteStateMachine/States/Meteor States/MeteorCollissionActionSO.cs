using UnityEngine;

[CreateAssetMenu(fileName = "MeteorCollissionActionSO", menuName = "Scriptable Objects/FSM/Action/MeteorCollissionActionSO")]
public class MeteorCollissionActionSO : ActionSO<MeteorContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;
    [SerializeField] private SoundDataSO _impactSound;
    [SerializeField] private Color _color = Color.red;

    public override void DrawGizmos(MeteorContext context)
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(context.FSM.transform.position, context.Agent.StatsSystem.GetStatValue<CollissionDistanceStatSO>());
    }

    public override void Execute(MeteorContext context)
    {
        foreach (var player in _playerRTS.Items)
        {
            if (context.FSM.transform.position.IsWithinRange(player.transform.position, context.Agent.StatsSystem.GetStatValue<CollissionDistanceStatSO>()))
            {
                if (player.TryGetComponent<IHealthSystem>(out var damageable))
                {
                    if (damageable.IsInvulnerable) return;
                    damageable.Damage((int)context.Agent.StatsSystem.GetStatValue<AttackStatSO>());
                    _impactSound.PlayEvent();
                }
            }
        }
    }
}
