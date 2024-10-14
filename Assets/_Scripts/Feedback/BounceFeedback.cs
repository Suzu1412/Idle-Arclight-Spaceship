using DG.Tweening;
using UnityEngine;

public class BounceFeedback : Feedback
{
    private Transform _transform;
    private Vector2 _originalSize;
    [SerializeField] [Range(0.5f, 1.5f)] private float _bounceSize = 0.95f;
    [SerializeField] private float _duration = 0.15f;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    private void Awake()
    {
        _transform = this.transform;
    }

    private void Start()
    {
        _originalSize = transform.localScale;
    }

    private void OnEnable()
    {
        HealthSystem.OnHit += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnHit -= StartFeedback;
    }


    public override void ResetFeedback()
    {
        _transform.localScale = _originalSize;
        transform.DOKill();
    }

    public override void StartFeedback()
    {
        ResetFeedback();
        _transform.DOScale(_originalSize * _bounceSize, _duration).SetLoops(2, LoopType.Yoyo);
    }

}
