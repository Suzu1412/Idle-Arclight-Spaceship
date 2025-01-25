using UnityEngine;

public interface ITargetDetector 
{
    Transform TargetTransform { get; }
    bool IsDetected { get; }
    bool IsVisibleToCamera { get; }
}
