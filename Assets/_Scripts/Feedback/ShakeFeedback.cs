using UnityEngine;
using DG.Tweening;

public class ShakeFeedback : Feedback
{
    private Transform _affectedTransform;
    [SerializeField, Range(0, 1)]
    private float _shakeStrength = 0.1f;
    [SerializeField, Range(0, 1)]
    private float _shakeDuration = 0.3f;
    [SerializeField, Range(0, 100)]
    private int _shakeVibrato = 30;
    private Vector2 _originalPosition;

    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();


    private void Awake()
    {
        _affectedTransform = this.transform;
        _originalPosition = this.transform.position;
    }

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
        _affectedTransform.DOKill();
        _affectedTransform.position = _originalPosition;
    }

    public override void StartFeedback()
    {
        ResetFeedback();
        _affectedTransform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
    }
}
