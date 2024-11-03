using UnityEngine;

public class TransformMover : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Vector2 _direction = Vector2.down;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _maxDurationTime = 15f;
    private float _durationTime;
    private ObjectPooler _pool;

    private void Awake()
    {
        _transform = this.transform;
    }
    private void Start()
    {
        _pool = GetComponent<ObjectPooler>();
    }

    private void OnEnable()
    {
        _durationTime = _maxDurationTime;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        _transform.Translate(_direction * _moveSpeed * Time.deltaTime);

        _durationTime -= Time.deltaTime;

        if (_durationTime <= 0f)
        {
            if (_pool != null)
            {
                ObjectPoolFactory.ReturnToPool(_pool);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

}

