using UnityEngine;

public abstract class BaseDetectionStrategy : IDetectionStrategy
{
    protected IAgent _agent;
    protected BaseDetectionStrategySO _detector;

    public void Initialize(IAgent agent, BaseDetectionStrategySO detector)
    {
        _agent = agent;
        _detector = detector;
    }

    public abstract Transform Detect(Transform detector, Vector2 direction, LayerMask target);

    public abstract void DrawGizmos(Transform detector, Vector2 direction, bool detected);
}
