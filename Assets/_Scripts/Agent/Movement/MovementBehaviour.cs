using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementBehaviour : MonoBehaviour, ICanMove
{
    private IAgent _agent;
    private Vector2 _direction;
    private Vector2 _currentSpeed;
    private Rigidbody2D _rb;
    [SerializeField] private MovementDataSO _data = default;

    public IAgent Agent => _agent ??= GetComponent<IAgent>();
    public Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        Move();
    }

    public void StopMovement()
    {
        ApplyVelocity(Vector2.zero);
    }

    public void Move()
    {
        Vector2 targetSpeed = TargetMoveSpeed(1f);
        float accelRate = BaseAcceleration(targetSpeed);

        // Calculate difference between current velocity and desired velocity
        Vector2 speedDif = targetSpeed - RB.linearVelocity;

        _currentSpeed = speedDif * accelRate;
        ApplyForce(_currentSpeed, ForceMode2D.Force);
    }

    public void ApplyVelocity(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
    }

    public void ApplyForce(Vector2 force, ForceMode2D forceMode)
    {
        RB.AddForce(force, forceMode);
    }

    public void ReadInputDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private Vector2 TargetMoveSpeed(float lerpAmount)
    {
        // Direction we want to move in and our desired velocity
        Vector2 targetSpeed = _direction * _data.MaxSpeed;

        // Smooth changes to are direction and speed
        targetSpeed = Vector2.Lerp(
          RB.linearVelocity, targetSpeed, lerpAmount
        );

        return targetSpeed;
    }

    private float BaseAcceleration(Vector2 targetSpeed)
    {
        // Acceleration value based on if we are
        // accelerating (includes turning) or trying to decelerate (stop).
        float accelRate = _direction != Vector2.zero ?
          _data.Acceleration : _data.Deceleration;

        return accelRate;
    }
}
