using UnityEngine;

[CreateAssetMenu(fileName = "Cone Detection Strategy", menuName = "Scriptable Objects/Detector/ConeDetectionStrategySO")]
public class ConeDetectionStrategySO : BaseDetectionStrategySO
{
    [SerializeField][Range(1f, 360f)] private float _detectionAngle = 60f; // Cone in front of enemy
    [SerializeField][Range(1f, 10f)] private float _detectionRadius = 5f; // Cone distance from enemy
    [SerializeField] private float _innerDetectionRadius = 5f; // Inner circle around enemy

    public float DetectionAngle => _detectionAngle;
    public float DetectionRadius => _detectionRadius;

    public override Transform Detect(IAgent agent, Transform detector, Vector2 direction, LayerMask target)
    {
        //RaycastHit2D hit = Physics2D.Raycast(detector.position, direction, target);

        RaycastHit2D hit = Physics2D.CircleCast(detector.position, _detectionRadius, Vector2.zero, Mathf.Infinity, target);

        if (hit.collider == null)
        {
            return null;
        }

        Vector2 playerVector = (hit.collider.transform.position - detector.position).normalized;
        if (Vector3.Angle(playerVector, direction) < _detectionAngle * 0.5f)
        {
            if (playerVector.magnitude < _detectionRadius)
            {
                return hit.collider.transform;
            }
        }

        return null;
    }

    public override void DrawGizmos(IAgent agent, Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? DetectedColor : UndetectedColor;
        // Circulo
        //Gizmos.DrawWireSphere(detector.position, _detectionRadius);

        float halfVisionAngle = _detectionAngle * 0.5f;

        Vector3 p1, p2;

        float angleDirection = 0;
        if (agent.FacingDirection.x < 0)
        {
            angleDirection = 0f;
        }
        else if (agent.FacingDirection.y < 0)
        {
            angleDirection = 270f;
        }
        else if (agent.FacingDirection.y > 0)
        {
            angleDirection = 90f;
        }
        else if (agent.FacingDirection.x > 0)
        {
            angleDirection = 180f;
        }

        p1 = PointForAngle(halfVisionAngle + angleDirection, _detectionRadius);
        p2 = PointForAngle(-halfVisionAngle + angleDirection, _detectionRadius);

        Gizmos.DrawLine(detector.position, detector.position + p1);
        Gizmos.DrawLine(detector.position, detector.position + p2);

        Gizmos.DrawRay(detector.position, direction * _detectionRadius);
    }

    Vector3 PointForAngle(float angle, float distance)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * distance;
    }

}