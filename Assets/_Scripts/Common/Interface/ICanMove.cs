using UnityEngine;

public interface ICanMove
{
    Rigidbody2D RB { get; }

    void SetMoveData(IMoveData moveData);

    /// <summary>
    /// Read Input Direction. Run in Update for correct working
    /// </summary>
    /// <param name="direction"></param>
    void ReadInputDirection(Vector2 direction);

    /// <summary>
    /// Stop All Velocities in Rigidbody
    /// </summary>
    void StopMovement();

    /// <summary>
    /// Apply Calculated Movement from Input Direction * Acceleration
    /// </summary>
    void Move();

    /// <summary>
    /// Apply Calculated Movement
    /// </summary>
    /// <param name="position"></param>
    void MoveTowards(Vector2 position);

    /// <summary>
    /// Apply Vector2 Velocity Directly to Rigidbody
    /// </summary>
    /// <param name="velocity"></param>
    void ApplyVelocity(Vector2 velocity);

    /// <summary>
    /// Apply Force Directly to Rigidbody
    /// </summary>
    /// <param name="force"></param>
    /// <param name="forceMode"></param>
    void ApplyForce(Vector2 force, ForceMode2D forceMode);

    void SetBoundaries(Vector2 boundary);

    void BoundMovement();

}