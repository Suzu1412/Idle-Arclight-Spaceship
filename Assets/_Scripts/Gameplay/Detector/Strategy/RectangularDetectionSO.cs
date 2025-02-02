using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Detection/Rectangular")]
public class RectangularDetectionSO : DetectionStrategySO
{
    public override void DrawGizmos(Transform origin, float range, float angle, Vector2 facingDirection)
    {
        Vector2 size = new(range, angle);
        Vector2 pos = origin.position;
        Vector2 forward = facingDirection.normalized;
        Vector2 right = new(forward.y, -forward.x);

        Vector2 topRight = pos + forward * (size.x / 2) + right * (size.y / 2);
        Vector2 topLeft = pos + forward * (size.x / 2) - right * (size.y / 2);
        Vector2 bottomRight = pos - forward * (size.x / 2) + right * (size.y / 2);
        Vector2 bottomLeft = pos - forward * (size.x / 2) - right * (size.y / 2);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
    }

    public override bool IsTargetDetected(Transform origin, Transform target, float range, float angle, Vector2 facingDirection)
    {
        Vector2 size = new(range, angle); // Using range as width and angle as height
        Vector2 toTarget = target.position - origin.position;
        Vector2 forward = facingDirection.normalized;
        Vector2 right = new(forward.y, -forward.x);

        float forwardDist = Vector2.Dot(toTarget, forward);
        float rightDist = Vector2.Dot(toTarget, right);

        return Mathf.Abs(forwardDist) <= size.x / 2 &&
               Mathf.Abs(rightDist) <= size.y / 2;
    }
}
