using System.Collections;
using UnityEngine;

public class TargetDetector : MonoBehaviour, ITargetDetector
{
    private IAgent _agent;
    [SerializeField] private GameObjectRuntimeSetSO _targetRTS;
    [SerializeField] private Color _detectedColor = Color.green;
    [SerializeField] private Color _undetectedColor = Color.red;
    [SerializeField] private float _minDistance = 2f;
    private bool _targetDetected = false;
    private Transform _transform;
    private Transform _targetTransform;
    private Coroutine _detectionCoroutine;

    public Transform TargetTransform => _targetTransform;

    public bool IsDetected => _targetDetected;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

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
            _targetTransform = null;
            _targetDetected = false;
            float closestDistance = float.MaxValue;

            foreach (var target in _targetRTS.Items)
            {
                // Inner Detection
                if (_transform.position.InRangeOf(target.transform.position, _minDistance))
                {
                    float distance = _transform.position.Distance(target.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        _targetTransform = target.transform;
                        _targetDetected = true;
                        continue;
                    }
                }

                // Detect within a Radius of the Target
                if (_transform.position.InRangeOf(target.transform.position, Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>()))
                {
                    // Detect in a Cone Shape Angle
                    float dot = Vector3.Dot(Agent.FacingDirection, _transform.position.DirectionToTarget(target.transform.position));
                    float coneAngle = Agent.StatsSystem.GetStatValue<DetectionAngleStatSO>();
                    if (dot > Mathf.Cos(coneAngle * Mathf.Deg2Rad))
                    {
                        float distance = _transform.position.Distance(target.transform.position);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            _targetTransform = target.transform;
                            _targetDetected = true;
                        }
                    }
                }
            }

            yield return Helpers.GetWaitForSeconds(0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw the Detection Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>());

        // Draw the Inner Detection Range
        Gizmos.DrawWireSphere(transform.position, _minDistance);

        // Draw the Forward Line
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Agent.FacingDirection * Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>());

        // Draw Tolerance Lines
        Quaternion leftRotation = Quaternion.Euler(0f, 0f, -Agent.StatsSystem.GetStatValue<DetectionAngleStatSO>());
        Quaternion rightRotation = Quaternion.Euler(0f, 0f, Agent.StatsSystem.GetStatValue<DetectionAngleStatSO>());

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRotation * Agent.FacingDirection * Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>());
        Gizmos.DrawRay(transform.position, rightRotation * Agent.FacingDirection * Agent.StatsSystem.GetStatValue<DetectionDistanceStatSO>());

    }
}
