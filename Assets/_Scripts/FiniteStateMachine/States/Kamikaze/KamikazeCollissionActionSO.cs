using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeCollissionActionSO", menuName = "Scriptable Objects/FSM/Action/KamikazeCollissionActionSO")]
public class KamikazeCollissionActionSO : ActionSO<KamikazeContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;
    [SerializeField] private SoundDataSO _impactSound;
    [SerializeField] private Color _color = Color.red;

    public override void DrawGizmos(KamikazeContext context)
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(context.FSM.transform.position, context.Agent.StatsSystem.GetStatValue<CollissionDistanceStatSO>());

    }

    public override void Execute(KamikazeContext context)
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
                    context.Agent.HealthSystem.Death(context.FSM.gameObject, DeathCauseType.Kamikaze);
                }
            }
        }
    }
}
