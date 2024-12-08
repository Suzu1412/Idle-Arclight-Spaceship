using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class ItemPickUp : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _pickableCollider;
    [SerializeField] private ItemSO _item;
    [SerializeField] private Color gizmoColor = Color.magenta;
    [SerializeField] private SoundDataSO _soundOnPickup;
    private Rigidbody2D _rb;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _lifeTime = 10f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 5f;
    private Vector2 _currentSpeed;
    private Transform _followTarget;
    private bool _hasRandomMovement;
    private bool _isMagnetDrawEnabled;
    private Vector2 _direction;
    private bool _isPickedUp;
    
    private bool _hasTarget = false;

    private ObjectPooler _pool;
    internal Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _pickableCollider = GetComponent<CircleCollider2D>();

        if (_pickableCollider == null)
        {
            Debug.Log($"Please Add Collider to: {this.gameObject}");
        }
    }

    private async void OnEnable()
    {
        transform.localScale = Vector3.one;
        _isMagnetDrawEnabled = false;
        _isPickedUp = false;
        await Awaitable.WaitForSecondsAsync(0.2f);
        _pickableCollider.enabled = true;
    }

    private void OnDisable()
    {
        _pickableCollider.enabled = false;
        _hasTarget = false;
        _followTarget = null;
        _isMagnetDrawEnabled = false;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetMaxSpeed(float movementSpeed)
    {
        _maxSpeed = Mathf.Clamp(movementSpeed, 0f, 5f);
    }

    public void SetItem(ItemSO item)
    {
        _item = item;
        _spriteRenderer.sprite = item.ItemImage;
    }

    public void EnableMagnet(bool value)
    {
        _isMagnetDrawEnabled = value;
        _hasTarget = false;
        _followTarget = null;
        
    }

    public void Magnet(Transform other)
    {
        if (!_isMagnetDrawEnabled) return;
        _followTarget = other;
        _hasTarget = true;
        _isMagnetDrawEnabled = false;
    }

    private void Move()
    {
        RB.AddForce(_direction, ForceMode2D.Force);
    }

    private void CalculateMovement() 
    {
        if (_hasTarget)
        {
            _direction = (_followTarget.position - transform.position).normalized;
        }
        else
        {
            _direction = Vector2.down;
        }

        Vector2 targetSpeed = TargetMoveSpeed(1f);
        float accelRate = BaseAcceleration(targetSpeed);

        // Calculate difference between current velocity and desired velocity
        Vector2 speedDif = targetSpeed - RB.linearVelocity;

        _currentSpeed = speedDif * accelRate;
        ApplyForce(_currentSpeed, ForceMode2D.Force);
    }

    private Vector2 TargetMoveSpeed(float lerpAmount)
    {
        // Direction we want to move in and our desired velocity
        Vector2 targetSpeed = _direction * _maxSpeed;

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
          _acceleration: _deceleration;

        return accelRate;
    }

    public void ApplyForce(Vector2 force, ForceMode2D forceMode)
    {
        RB.AddForce(force, forceMode);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IAgent>(out var agent))
        {
            if (_isPickedUp) return;
            _item.PickUp(agent);
            _soundOnPickup?.PlayEvent();
            _isPickedUp = true;
            ObjectPoolFactory.ReturnToPool(Pool);
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _pickableCollider.radius);
    }
}
