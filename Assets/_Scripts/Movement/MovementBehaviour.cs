using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementBehaviour : MonoBehaviour, ICanMove
{
    private Camera _camera;
    private IAgent _agent;
    private Vector2 _direction;
    private Vector2 _currentSpeed;
    private Vector2 _targetPosition;

    private float _leftBounds = 0;
    private float _rightBounds = 0;
    private float _topBounds = 0;
    private float _bottomBounds = 0;

    private Rigidbody2D _rb;
    private bool _hasBoundaries;
    [SerializeField] private MovementDataSO _data = default;

    public IAgent Agent => _agent ??= GetComponent<IAgent>();
    public Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(SetAutomaticBoundaries());
    }

    public void SetMovementData(MovementDataSO data)
    {
        _data = data;
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

    private IEnumerator SetAutomaticBoundaries()
    {
        yield return Helpers.GetWaitForSeconds(0.1f);

        if (_camera == null)
        {
            Debug.LogError("No Camera in Scene. Please Fix");
        }
        _leftBounds = _camera.ViewportToWorldPoint(new Vector2(0.1f, 0f)).x;
        _rightBounds = _camera.ViewportToWorldPoint(new Vector2(0.9f, 0f)).x;
        _bottomBounds = _camera.ViewportToWorldPoint(new Vector2(0, 0.05f)).y;
        _topBounds = _camera.ViewportToWorldPoint(new Vector2(0, 0.95f)).y;
        _hasBoundaries = true;
    }
}
