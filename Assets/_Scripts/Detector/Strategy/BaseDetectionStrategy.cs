using UnityEngine;

public abstract class BaseDetectionStrategy
{
    protected IAgent _agent;
    protected BaseDetectionStrategySO _detector;

    public virtual void Initialize(IAgent agent, BaseDetectionStrategySO detector)
    {
        _agent = agent;
        _detector = detector;
    }
}
