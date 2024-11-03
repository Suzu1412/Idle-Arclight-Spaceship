using UnityEngine;

public abstract class BaseDetectionStrategySO : ScriptableObject
{
    [SerializeField] private Color _detectedColor = Color.green;
    [SerializeField] private Color _undetectedColor = Color.red;
    [SerializeField] [Range(0.05f, 1f)] protected float _detectionCooldown = 0.1f;
    public float DetectionCooldown => _detectionCooldown;
    public Color DetectedColor => _detectedColor;
    public Color UndetectedColor => _undetectedColor;


    public abstract BaseDetectionStrategy CreateDetector();
}

public abstract class BaseDetectionStrategySO<T> : BaseDetectionStrategySO where T : BaseDetectionStrategy, new()
{
    public override BaseDetectionStrategy CreateDetector() => new T();
}