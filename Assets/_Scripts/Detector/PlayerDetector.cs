using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private IAgent _agent;
    private Transform _transform;
    private LayerMask _targetMask;

    [SerializeField] private float _detectionAngle = 60f; // Cone in front of enemy
    [SerializeField] private float _detectionRadius = 10f; // Cone distance from enemy
    [SerializeField] private float _innerDetectionRadius = 5f; // Inner circle around enemy
    [SerializeField] private float _detectionCooldown = 1f; // Time between detection
    [SerializeField] private BaseDetectionStrategySO _detectionStrategySO;
    private BaseDetectionStrategy _detectionStrategy;
    public Transform Player { get; }
    public bool IsDetected => Player != null;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();


    private void Awake()
    {
        _detectionStrategy = _detectionStrategySO.CreateDetector();
        _detectionStrategy.Initialize(Agent, _detectionStrategySO);
        _transform = this.transform;
        _targetMask = LayerMask.NameToLayer("Player");
    }

    private void OnDrawGizmos()
    {
        if (_detectionStrategy == null) return;
        _detectionStrategy.DrawGizmos(_transform, Agent.FacingDirection, IsDetected);
    }

}
