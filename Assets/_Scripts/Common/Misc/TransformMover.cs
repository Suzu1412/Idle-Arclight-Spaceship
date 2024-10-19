using UnityEngine;
using UnityEngine.Experimental.AI;

public class TransformMover : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Vector2 _direction = Vector2.down;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private Vector2 _deadZone = new Vector2(0f, -5f);
    private ObjectPooler _pool;

    private void Awake()
    {
        _transform = this.transform;
    }
    private void Start()
    {
        _pool = GetComponent<ObjectPooler>();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void SetDeadZone(Vector2 deadZone)
    {
        _deadZone = deadZone;
    }

    private void Update()
    {
        _transform.Translate(_direction * _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(_transform.position, _deadZone) < 0.2f)
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

