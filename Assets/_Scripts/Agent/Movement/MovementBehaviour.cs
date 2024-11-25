using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementBehaviour : MonoBehaviour, ICanMove
{
    private IAgent _agent;
    private Vector2 _direction;
    private Vector2 _currentSpeed;
    private Vector2 _targetPosition;

    [SerializeField] private float _leftBounds = -2.3f;
    [SerializeField] private float _rightBounds = 2.3f;
    [SerializeField] private float _topBounds = 3f;
    [SerializeField] private float _bottomBounds = -3f;

    private Rigidbody2D _rb;
    [SerializeField] private bool _hasBoundaries;
    private IMoveData _moveData;

    public IAgent Agent => _agent ??= GetComponent<IAgent>();
    public Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();

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
        Vector2 targetSpeed = _direction * _moveData.MaxSpeed;

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
          _moveData.Acceleration : _moveData.Deceleration;

        return accelRate;
    }

    public void MoveTowards(Vector2 position)
    {
        _targetPosition = position;
        Debug.Log(_targetPosition);
        StopAllCoroutines();
        StartCoroutine(MoveTowardsCoroutine());
    }

    public void BoundMovement()
    {
        if (_hasBoundaries)
        {
            Vector2 restrictMovement = Vector2.zero;
            restrictMovement.x = Mathf.Clamp(RB.position.x, _leftBounds, _rightBounds);
            restrictMovement.y = Mathf.Clamp(RB.position.y, _bottomBounds, _topBounds);
            RB.position = restrictMovement;
        }
    }

    private IEnumerator MoveTowardsCoroutine()
    {
        bool targetReached = false;

        while (!targetReached)
        {
            Vector2 targetDirection = _targetPosition - (Vector2)transform.position;
            ReadInputDirection(targetDirection.normalized);

            if (Vector3.Distance(RB.position, _targetPosition) < 0.2f)
            {
                RB.position = _targetPosition;
                ReadInputDirection(Vector2.zero);
                targetReached = true;
            }

            yield return null;
        }
    }

    public void SetBoundaries(Vector2 boundary)
    {
        _leftBounds = -boundary.x;
        _rightBounds = boundary.x;
        _bottomBounds = -boundary.y;
        _topBounds = boundary.y;
    }

    public void SetMoveData(IMoveData moveData)
    {
        _moveData = moveData;
    }
}
