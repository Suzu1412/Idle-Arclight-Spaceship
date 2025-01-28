using UnityEngine;

public class VibrationFeedback : Feedback
{
    [SerializeField] private BoolVariableSO _CanVibrate;
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
        if (!_CanVibrate.Value) return;
        Handheld.Vibrate();

    }
}
