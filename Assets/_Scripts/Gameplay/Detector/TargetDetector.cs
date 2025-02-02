using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TargetDetector : MonoBehaviour, ITargetDetector
{
    private IAgent _agent;
    [SerializeField] private DetectionStrategySO _detectionStrategy;
    [SerializeField] private GameObjectRuntimeSetSO _targetRTS;
    private bool _targetDetected = false;
    private bool _isVisibleToCamera = false;
    private float _closestTarget;
    private Transform _transform;
    private Transform _targetTransform;
    private Coroutine _detectionCoroutine;

    public Transform TargetTransform => _targetTransform;

    public bool IsDetected => _targetDetected;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    public bool IsVisibleToCamera => _isVisibleToCamera;

    private Camera _mainCamera;

    private void Awake()
    {
        _transform = this.transform;
    }

    private void Start()
    {
        if (_targetRTS == null)
        {
            Debug.LogError($"{gameObject.name} has no Target RTS attached. Please Fix");
        }

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Detect();
    }

    public void Detect()
    {
        if (_detectionStrategy == null)
        {
            Debug.Log("Detection missing from " + transform.parent.gameObject);
        }

        _targetDetected = false;
        _targetTransform = null;
        _closestTarget = Mathf.Infinity;

        foreach (var target in _targetRTS.Items)
        {
            if (!CheckIsVisibleToCamera(target.transform.position)) continue;

            var detectionRange = Agent.StatsSystem.GetStat<DetectionRangeStatSO>().Value;
            var detectionAngle = Agent.StatsSystem.GetStat<DetectionAngleStatSO>().Value;

            if (!_detectionStrategy.IsTargetDetected(transform, target.transform, detectionRange, detectionAngle, Agent.FacingDirection)) continue;

            float distanceSquared = _transform.position.GetSquaredDistanceTo(target.transform.position);

            if (distanceSquared < _closestTarget)
            {
                _closestTarget = distanceSquared;
                _targetTransform = target.transform;
                _targetDetected = true;
                continue;
            }
        }
    }

    private bool CheckIsVisibleToCamera(Vector3 targetPosition)
    {
        if (_mainCamera == null) return false;
        // Convert the target position to viewport space
        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(targetPosition);

        // Check if the target is within the bounds of the camera's viewport
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (_detectionStrategy == null) return;

        var detectionRange = Agent.StatsSystem.GetStat<DetectionRangeStatSO>().Value;
        var detectionAngle = Agent.StatsSystem.GetStat<DetectionAngleStatSO>().Value;

        _detectionStrategy.DrawGizmos(transform, detectionRange, detectionAngle, Agent.FacingDirection);
    }
}
