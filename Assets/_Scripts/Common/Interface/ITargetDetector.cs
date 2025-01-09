using UnityEngine;

public interface ITargetDetector 
{
    Transform TargetTransform { get; }
    bool IsDetected { get; }
}
