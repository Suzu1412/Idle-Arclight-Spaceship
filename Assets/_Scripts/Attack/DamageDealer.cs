using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool _destroyOnContact = false;
    private ObjectPooler _pool;
    private IAgent _agent;
    internal IAgent Agent => _agent ??= GetComponentInParent<IAgent>();

    private void Start()
    {
        _pool = GetComponent<ObjectPooler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHittable>(out var hittable))
        {
            if (hittable.IsInvulnerable) return;
            hittable.GetHit(this.gameObject);
        }
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage((int)Agent.GetStat(StatType.Strength));

            if (_destroyOnContact)
            {
                Agent.HealthSystem.Death();
            }
        }
    }
}
