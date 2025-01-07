using UnityEngine;

public class EnemyUIFollower : MonoBehaviour
{
    [SerializeField] private Transform _enemyPosition;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        if (_enemyPosition != null)
        {
            transform.position = _enemyPosition.position + _offset;
        }
    }


}
