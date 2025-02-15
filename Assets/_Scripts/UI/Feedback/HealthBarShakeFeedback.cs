using UnityEngine;

public class HealthBarShakeFeedback : Feedback
{
    [SerializeField] private VoidGameEvent OnHealthBarShakeAnimationEvent;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();


    private void OnEnable()
    {
        HealthSystem.OnHit += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnHit -= StartFeedback;
        ResetFeedback();
    }

    public override void ResetFeedback()
    {
    }

    public override void StartFeedback()
    {
        OnHealthBarShakeAnimationEvent.RaiseEvent(this);
    }
}
