using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class GemMovement : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Vector2 _direction = Vector2.down;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _maxDurationTime = 15f;
    private float _durationTime;
    private ObjectPooler _pool;





}


