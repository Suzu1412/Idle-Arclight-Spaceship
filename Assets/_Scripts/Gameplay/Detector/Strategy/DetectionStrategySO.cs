using UnityEngine;

public abstract class DetectionStrategySO : ScriptableObject
{
    public abstract bool IsTargetDetected(Transform origin, Transform target, float range, float angle, Vector2 facingDirection);
    public abstract void DrawGizmos(Transform origin, float range, float angle, Vector2 facingDirection);
}