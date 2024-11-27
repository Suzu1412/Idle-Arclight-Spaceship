using UnityEngine;

public interface IMoveData
{
    float Acceleration { get; }
    float Deceleration { get; }
    float MaxSpeed { get; }
}
