using UnityEngine;

[CreateAssetMenu(fileName = "Circle Detection Strategy", menuName = "Scriptable Objects/Detector/CircleDetectionStrategySO")]
public class CircleDetectionStrategySO : BaseDetectionStrategySO<CircleDetectionStrategy>
{
    [SerializeField][Range(1f, 10f)] private float _detectionRadius = 5f; // Circle distance from enemy

    public float DetectionRadius => _detectionRadius;
}

public class CircleDetectionStrategy : BaseDetectionStrategy
{
    private float _detectionRadius;

    public override void Initialize(IAgent agent, BaseDetectionStrategySO detector)
    {
        base.Initialize(agent, detector);
        _detectionRadius = (detector as CircleDetectionStrategySO).DetectionRadius;
    }

    public override Transform Detect(Transform detector, Vector2 direction, LayerMask target)
    {
        RaycastHit2D hit = Physics2D.CircleCast(detector.position, _detectionRadius, Vector2.zero, Mathf.Infinity, target);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.transform;
    }

    public override void DrawGizmos(Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? _detector.DetectedColor : _detector.UndetectedColor;
        Gizmos.DrawWireSphere(detector.position, _detectionRadius);
    }
}
