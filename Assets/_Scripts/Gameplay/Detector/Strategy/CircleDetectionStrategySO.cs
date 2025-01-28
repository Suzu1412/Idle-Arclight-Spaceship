using UnityEngine;

[CreateAssetMenu(fileName = "Circle Detection Strategy", menuName = "Scriptable Objects/Detector/CircleDetectionStrategySO")]
public class CircleDetectionStrategySO : BaseDetectionStrategySO
{
    [SerializeField][Range(1f, 10f)] private float _detectionRadius = 5f; // Circle distance from enemy

    public float DetectionRadius => _detectionRadius;

    public override Transform Detect(IAgent agent, Transform detector, Vector2 direction, LayerMask target)
    {
        RaycastHit2D hit = Physics2D.CircleCast(detector.position, _detectionRadius, Vector2.zero, Mathf.Infinity, target);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.transform;
    }

    public override void DrawGizmos(IAgent agent, Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? DetectedColor : UndetectedColor;
        Gizmos.DrawWireSphere(detector.position, _detectionRadius);
    }
}
