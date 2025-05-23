using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool _destroyOnContact = false;
    [SerializeField] private SoundDataSO _impactSound;
    private ObjectPooler _pool;
    private IAgent _agent;
    internal IAgent Agent => _agent ??= GetComponentInParent<IAgent>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHittable>(out var hittable))
        {
            if (hittable.IsInvulnerable) return;
            hittable.GetHit(this.gameObject);
        }
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if (Agent == null) return;
            damageable.Damage((int)Agent.StatsSystem.GetStatValue<AttackStatSO>());
            _impactSound.PlayEvent();
        }
    }
}
