using System.Collections;
using UnityEngine;

public class ShakeFeedback : Feedback
{
    private Transform _targetTransform;
    [SerializeField, Range(0, 1)]
    private float _magnitude = 0.1f;
    [SerializeField, Range(0, 1)]
    private float _duration = 0.3f;
    private Vector2 _originalPosition;
    private Coroutine _shakeCoroutine;

    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();


    void Start()
    {
        if (_targetTransform == null)
            _targetTransform = transform; // Default to this object's transform
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
        _targetTransform.localPosition = _originalPosition;
    }

    public override void StartFeedback()
    {
        ResetFeedback();
        if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        _originalPosition = _targetTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            float offsetX = Random.Range(-1f, 1f) * _magnitude;
            float offsetY = Random.Range(-1f, 1f) * _magnitude;

            _targetTransform.localPosition = new Vector3(_originalPosition.x + offsetX, _originalPosition.y + offsetY, _targetTransform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset to the original position
        _targetTransform.localPosition = _originalPosition;
    }
}
