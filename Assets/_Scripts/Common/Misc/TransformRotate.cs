using UnityEditor.Graphs;
using UnityEngine;

public class TransformRotate : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _speed = 30f;

    private void Awake()
    {
        _transform = this.transform;
    }

    private void Update()
    {
        _transform.Rotate(0f, 0f, _speed * Time.deltaTime);
    }
}
