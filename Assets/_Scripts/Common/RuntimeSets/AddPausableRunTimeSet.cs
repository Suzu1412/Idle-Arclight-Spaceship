using UnityEngine;

public class AddPausableRunTimeSet : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSetSO _pauseRunTimeSet = default;

    private void OnEnable()
    {
        _pauseRunTimeSet.Add(gameObject);
    }

    private void OnDisable()
    {
        _pauseRunTimeSet.Remove(gameObject);
    }
}
