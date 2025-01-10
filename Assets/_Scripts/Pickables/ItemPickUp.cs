using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class ItemPickUp : MonoBehaviour, IPausable, IRemovable
{
    [SerializeField] private PausableRunTimeSetSO _pausable;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _pickableCollider;
    [SerializeField] private ItemSO _item;
    [SerializeField] private Color gizmoColor = Color.magenta;
    [SerializeField] private SoundDataSO _soundOnPickup;
    [SerializeField] private ItemMovementSO _movement;
    private Rigidbody2D _rb;
    private ItemBehaviour _behaviour;

    private Vector2 _currentSpeed;
    private Transform _followTarget;
    private Vector2 _direction;
    private bool _isPickedUp;
    private float _spawnDuration;
    private float _stillDuration;
    private bool _isPaused;
    
    private bool _hasTarget = false;


    public event Action OnRemove;

    public Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _pickableCollider = GetComponent<CircleCollider2D>();

        if (_pickableCollider == null)
        {
            Debug.Log($"Please Add Collider to: {this.gameObject}");
        }
    }

    private void OnEnable()
    {
        _pausable.Add(this);
        transform.localScale = Vector3.one;
        _isPickedUp = false;
        _spawnDuration = 0.3f;
        _stillDuration = 1f;
        _behaviour = ItemBehaviour.Spawning;
        _pickableCollider.enabled = true;
    }

    private void OnDisable()
    {
        _pausable.Remove(this);
        _hasTarget = false;
        _followTarget = null;
    }

    private void Update()
    {
        if (_isPaused) return;
        CalculateDirection();
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;
        Move();
    }

    public void SetItem(ItemSO item)
    {
        _item = item;
        _spriteRenderer.sprite = item.ItemImage;
    }

    public void Magnet(Transform other)
    {
        _followTarget = other;
        _hasTarget = true;

        if (_hasTarget && _followTarget != null)
        {
            _behaviour = ItemBehaviour.Following;
        }
    }

    private void Move()
    {
        RB.AddForce(_direction, ForceMode2D.Force);
    }

    private void CalculateDirection()
    {
        switch (_behaviour)
        {
            case ItemBehaviour.Spawning:
                _spawnDuration -= Time.deltaTime;
                _direction = Vector2.up;

                if (_spawnDuration <= 0f)
                {
                    _behaviour = ItemBehaviour.Still;
                }

                break;

            case ItemBehaviour.Still:
                _stillDuration -= Time.deltaTime;
                _direction = Vector2.zero;

                if (_stillDuration <= 0f)
                {
                    _behaviour = ItemBehaviour.Falling;
                }
                break;

            case ItemBehaviour.Falling:
                _direction = Vector2.down;
                break;

            case ItemBehaviour.Following:
                if (_followTarget == null)
                {
                    _behaviour = ItemBehaviour.Falling;
                    break;
                }

                _direction = (_followTarget.position - transform.position).normalized * 1.5f;
                break;
        }

    }

    private void CalculateMovement() 
    {
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
        Vector2 targetSpeed = _direction * _movement.MaxSpeed;

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
          _movement.Acceleration: _movement.Deceleration;

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
            Remove(this.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _pickableCollider.radius);
    }

    public void Pause(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            RB.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            RB.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Remove(GameObject source)
    {
        if (!IsValidSource(source)) return;

        OnRemove.Invoke();
    }

    private enum ItemBehaviour
    {
        Spawning,
        Still,
        Falling,
        Following
    }

    private bool IsValidSource(GameObject source)
    {
        return source == gameObject || source.CompareTag("DeadZone");
    }
}
