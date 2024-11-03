using UnityEngine;

public interface IDetectionStrategy
{
    Transform Detect(Transform detector, Vector2 direction, LayerMask target);

    void DrawGizmos(Transform detector, Vector2 direction, bool detected);
}
