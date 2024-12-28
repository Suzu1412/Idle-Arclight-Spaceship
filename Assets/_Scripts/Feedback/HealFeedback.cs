using TreeEditor;
using UnityEngine;

public class HealFeedback : Feedback
{
    [SerializeField] private Animator _healAnimator;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    private void OnEnable()
    {
        HealthSystem.OnHeal += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnHeal -= StartFeedback;
        ResetFeedback();
    }

    public override void ResetFeedback()
    {
        _healAnimator.gameObject.SetActive(false);
    }

    public override void StartFeedback()
    {
        _healAnimator.gameObject.SetActive(true);
    }
}
