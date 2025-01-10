using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeCollissionActionSO", menuName = "Scriptable Objects/FSM/Action/KamikazeCollissionActionSO")]
public class KamikazeCollissionActionSO : ActionSO
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;
    [SerializeField] private SoundDataSO _impactSound;
    [SerializeField] private float _range = 1f;
    [SerializeField] private Color _color = Color.green;

    public override void DrawGizmos(FiniteStateMachine fsm)
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(fsm.transform.position, _range);
    }

    public override void Execute(FiniteStateMachine fsm)
    {
        foreach (var player in _playerRTS.Items)
        {
            if (fsm.transform.position.IsWithinRange(player.transform.position, _range))
            {
                if (player.TryGetComponent<IHealthSystem>(out var damageable))
                {
                    if (damageable.IsInvulnerable) return;
                    damageable.Damage((int)fsm.Agent.StatsSystem.GetStatValue<AttackStatSO>());
                    _impactSound.PlayEvent();
                    fsm.Agent.HealthSystem.Death(fsm.gameObject, DeathCauseType.Kamikaze);
                }
            }
        }
    }
}
