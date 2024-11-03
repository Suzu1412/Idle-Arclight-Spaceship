using UnityEngine;

[CreateAssetMenu(fileName = "Cone Detection Strategy", menuName = "Scriptable Objects/Detector/ConeDetectionStrategySO")]
public class ConeDetectionStrategySO : BaseDetectionStrategySO<ConeDetectionStrategy>
{
}

public class ConeDetectionStrategy : BaseDetectionStrategy
{


    public override Transform Detect(Transform detector, Vector2 direction, LayerMask target)
    {
        //RaycastHit2D hit = Physics2D.Raycast(detector.position, direction, target);

        RaycastHit2D hit = Physics2D.CircleCast(detector.position, 5f, Vector2.zero, target);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider);
        }

        return null;
    }

    public override void DrawGizmos(Transform detector, Vector2 direction, bool detected)
    {
        Gizmos.color = detected ? _detector.DetectedColor : _detector.UndetectedColor;
        Gizmos.DrawWireSphere(detector.position, 5f);
    }
}