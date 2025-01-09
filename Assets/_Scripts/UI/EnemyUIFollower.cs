using UnityEngine;

public class EnemyUIFollower : MonoBehaviour
{
    [SerializeField] private Transform _enemyPosition;
    [SerializeField] private Vector3 _offset;

    private void OnEnable()
    {
        if (_enemyPosition != null)
        {
            transform.position = _enemyPosition.position + _offset;
        }
    }

    private void Update()
    {
        if (_enemyPosition != null)
        {
            transform.position = _enemyPosition.position + _offset;
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position + _offset;
    }
}
