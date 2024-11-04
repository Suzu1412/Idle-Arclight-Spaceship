using UnityEngine;

[CreateAssetMenu(fileName = "Ray Detection Strategy", menuName = "Scriptable Objects/Detector/RayDetectionStrategySO")]
public class RayDetectionStrategySO : BaseDetectionStrategySO<RayDetectionStrategy>
{
    [SerializeField][Range(1f, 10f)] private float _detectionDistance = 5f; // Circle distance from enemy
    public float DetectionDistance => _detectionDistance;
}

public class RayDetectionStrategy : BaseDetectionStrategy
{
    private float _detectionDistance;

    public override void Initialize(IAgent agent, BaseDetectionStrategySO detector)
    {
        base.Initialize(agent, detector);
        _detectionDistance = (detector as RayDetectionStrategySO).DetectionDistance;
    }

    public override Transform Detect(Transform detector, Vector2 direction, LayerMask target)
    {
        RaycastHit2D hit = Physics2D.Raycast(detector.position, direction, _detectionDistance, target);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.transform;
    }

    public override void DrawGizmos(Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? _detector.DetectedColor : _detector.UndetectedColor;
        Gizmos.DrawRay(detector.position, direction * _detectionDistance);
    }
}