using UnityEngine;

public interface IDetectionStrategy
{
    Transform Detect(IAgent agent, Transform detector, Vector2 direction, LayerMask target);

    void DrawGizmos(IAgent agent, Transform detector, Vector2 direction, bool detected);
}
