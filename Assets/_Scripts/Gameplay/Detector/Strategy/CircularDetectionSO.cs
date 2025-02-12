using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Detection/Circular")]
public class CircularDetectionSO : DetectionStrategySO
{
    public override void DrawGizmos(Transform origin, float range, float angle, Vector2 facingDirection)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin.position, range);
    }

    public override bool IsTargetDetected(Transform origin, Transform target, float range, float angle, Vector2 facingDirection)
    {
        return Vector2.Distance(origin.position, target.position) <= range;

    }
}
