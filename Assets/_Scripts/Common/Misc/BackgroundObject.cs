using UnityEngine;

public class BackgroundObject : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private BoolVariableSO _isPaused;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction = Vector2.down;
    private float _moveSpeed;
    private Rigidbody2D _rb;

    public Rigidbody2D RB => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();

    public BoolVariableSO IsPaused => _isPaused;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (_isPaused.Value)
        {
            return;
        }
        RB.linearVelocity = _direction * _moveSpeed;
    }

    public void SetScale(Vector3 scale)
    {
        _transform.localScale = scale;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetOrderInLayer(int sortingOrder)
    {
        _spriteRenderer.sortingOrder = sortingOrder;
    }

    public void Pause(bool isPaused)
    {
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
}
