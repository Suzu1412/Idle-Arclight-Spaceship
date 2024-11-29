using UnityEngine;

[CreateAssetMenu(fileName = "Ray Detection Strategy", menuName = "Scriptable Objects/Detector/RayDetectionStrategySO")]
public class RayDetectionStrategySO : BaseDetectionStrategySO
{
    [SerializeField][Range(1f, 10f)] private float _detectionDistance = 5f; // Circle distance from enemy
    public float DetectionDistance => _detectionDistance;

    public override Transform Detect(IAgent agent, Transform detector, Vector2 direction, LayerMask target)
    {
        RaycastHit2D hit = Physics2D.Raycast(detector.position, direction, _detectionDistance, target);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.transform;
    }

    public override void DrawGizmos(IAgent agent, Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? DetectedColor : UndetectedColor;
        Gizmos.DrawRay(detector.position, direction * _detectionDistance);
    }
}