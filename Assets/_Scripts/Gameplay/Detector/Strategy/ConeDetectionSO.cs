using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Detection/Cone")]
public class ConeDetectionSO : DetectionStrategySO
{
    public override void DrawGizmos(Transform origin, float range, float angle, Vector2 facingDirection)
    {
        Gizmos.color = Color.yellow;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, -angle / 2) * facingDirection * range;
        Vector2 rightBoundary = Quaternion.Euler(0, 0, angle / 2) * facingDirection * range;

        Gizmos.DrawLine(origin.position, (Vector3)origin.position + (Vector3)leftBoundary);
        Gizmos.DrawLine(origin.position, (Vector3)origin.position + (Vector3)rightBoundary);
    }

    public override bool IsTargetDetected(Transform origin, Transform target, float range, float angle, Vector2 facingDirection)
    {
        Vector2 toTarget = (target.position - origin.position).normalized;
        float angleToTarget = Vector2.Angle(facingDirection, toTarget);

        return angleToTarget <= angle / 2 && Vector2.Distance(origin.position, target.position) <= range;
    }
}
