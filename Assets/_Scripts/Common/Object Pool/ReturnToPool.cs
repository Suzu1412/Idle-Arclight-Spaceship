using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    private ObjectPooler _pool;
    private IRemovable _removable;
    internal IRemovable Removable => _removable ??= gameObject.GetInterfaceInSelfOrChildren<IRemovable>();

    private void Start()
    {
        _pool = GetComponent<ObjectPooler>();
    }

    private void OnEnable()
    {
        Removable.OnRemove += Return;
    }

    private void OnDisable()
    {
        Removable.OnRemove -= Return;
    }

    private void Return()
    {
        if (_pool == null)
        {
            if (TryGetComponent<ObjectPooler>(out var pool))
            {
                _pool = pool;
            }
            else
            {
                Destroy(gameObject);
                Debug.Log(gameObject + " has no pool assigned");
                return;
            }
        }

        ObjectPoolFactory.ReturnToPool(_pool);
    }
}
