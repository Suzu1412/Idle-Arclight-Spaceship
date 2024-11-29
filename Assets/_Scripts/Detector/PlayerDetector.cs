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
    public bool IsDetected => PlayerDetected != null;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    public Transform PlayerDetected => _playerDetected;

    private void Awake()
    {
        _transform = this.transform;
    }

    private void OnEnable()
    {
        if (_detectionCoroutine != null) StopCoroutine(_detectionCoroutine);
        _detectionCoroutine = StartCoroutine(DetectionCoroutine());
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            _playerDetected = _detectionStrategySO.Detect(Agent, _transform, Agent.FacingDirection, _targetMask);
            yield return Helpers.GetWaitForSeconds(_detectionStrategySO.DetectionCooldown);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_detectionStrategySO == null) return;
        _detectionStrategySO.DrawGizmos(_agent, transform, Agent.FacingDirection, IsDetected);
    }

}
