using System.Runtime.CompilerServices;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private Vector2 _initialPosition;
    [SerializeField] private Vector2 _endPosition;
    [SerializeField] private Vector3 _moveSpeed = Vector2.one;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position += _moveSpeed * Time.deltaTime;

        if (Vector2.Distance(_transform.position, _endPosition) < 0.2f)
        {
            _transform.position = _initialPosition;
        }
    }
}
