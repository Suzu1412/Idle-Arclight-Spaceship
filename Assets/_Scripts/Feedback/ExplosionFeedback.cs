using UnityEngine;

public class ExplosionFeedback : Feedback
{
    [SerializeField] private ObjectPoolSettingsSO _deathVFX;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();


    private void OnEnable()
    {
        HealthSystem.OnDeath += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnDeath -= StartFeedback;
    }


    public override void ResetFeedback()
    {
    }

    public override void StartFeedback()
    {
        ObjectPoolFactory.Spawn(_deathVFX).transform.SetPositionAndRotation(transform.parent.position, transform.parent.rotation);
    }
}
