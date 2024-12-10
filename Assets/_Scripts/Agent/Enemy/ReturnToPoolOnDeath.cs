using UnityEngine;

public class ReturnToPoolOnDeath : MonoBehaviour
{
    private ObjectPooler _pool;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInChildren<IHealthSystem>();

    private void Start()
    {
        _pool = GetComponentInParent<ObjectPooler>();
    }

    private void OnEnable()
    {
        HealthSystem.OnDestroyGO += ReturnToPool;
    }

    private void OnDisable()
    {
        HealthSystem.OnDestroyGO -= ReturnToPool;
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            ObjectPoolFactory.ReturnToPool(_pool);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
