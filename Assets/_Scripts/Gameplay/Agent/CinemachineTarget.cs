using UnityEngine;
using Unity.Cinemachine;

public class CinemachineTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private CinemachineCamera _cm;

    private void Awake()
    {
        _cm = FindAnyObjectByType<CinemachineCamera>();
    }

    void OnEnable()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        if (_cm != null)
        {
            _cm.Follow = _target;
        }
    }

}
