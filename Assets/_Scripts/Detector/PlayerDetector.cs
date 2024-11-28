using System.Collections;
using UnityEngine;

public class PlayerDetector : MonoBehaviour, IPlayerDetector
{
    private IAgent _agent;
    private Transform _transform;
    private Transform _playerDetected;
    [SerializeField] private LayerMask _targetMask;
    private Coroutine _detectionCoroutine;
    [SerializeField] private BaseDetectionStrategySO _detectionStrategySO;
    private BaseDetectionStrategy _detectionStrategy;
    public bool IsDetected => PlayerDetected != null;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    public Transform PlayerDetected => _playerDetected;

    private void Awake()
    {
        _detectionStrategy = _detectionStrategySO.CreateDetector();
        _detectionStrategy.Initialize(Agent, _detectionStrategySO);
        _transform = this.transform;
    }

    private void OnEnable()
    {
        if (_detectionCoroutine != null) StopCoroutine(_detectionCoroutine);
        _detectionCoroutine = StartCoroutine(DetectionCoroutine());
    }

    private IEnumerator DetectionCoroutine()
    {
        if (_detectionStrategy == null)
        {
            Debug.LogError($"Detection Strategy not assigned to {gameObject}");
            yield return null;
        }

        while (true)
        {
            _playerDetected = _detectionStrategy.Detect(_transform, Agent.FacingDirection, _targetMask);
            yield return Helpers.GetWaitForSeconds(_detectionStrategySO.DetectionCooldown);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_detectionStrategy == null) return;
        _detectionStrategy.DrawGizmos(transform, Agent.FacingDirection, IsDetected);
    }

}
