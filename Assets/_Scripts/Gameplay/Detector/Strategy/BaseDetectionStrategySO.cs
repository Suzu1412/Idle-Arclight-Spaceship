using UnityEngine;

public abstract class BaseDetectionStrategySO : ScriptableObject, IDetectionStrategy
{
    [SerializeField] private Color _detectedColor = Color.green;
    [SerializeField] private Color _undetectedColor = Color.red;
    [SerializeField] [Range(0.001f, 1f)] protected float _detectionCooldown = 0.1f;
    public float DetectionCooldown => _detectionCooldown;
    public Color DetectedColor => _detectedColor;
    public Color UndetectedColor => _undetectedColor;

    public abstract Transform Detect(IAgent agent, Transform detector, Vector2 direction, LayerMask target);

    public abstract void DrawGizmos(IAgent agent, Transform detector, Vector2 direction, bool detected);
}