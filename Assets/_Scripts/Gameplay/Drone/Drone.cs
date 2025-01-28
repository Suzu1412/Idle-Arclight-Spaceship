using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private Transform _player;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _transform.Rotate(Vector3.forward, _speed * Time.deltaTime);  
    }
}
