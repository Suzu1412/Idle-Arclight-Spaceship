using UnityEngine;

public interface IPlayerDetector
{
    Transform PlayerDetected { get; }
    bool IsDetected { get; }
}
