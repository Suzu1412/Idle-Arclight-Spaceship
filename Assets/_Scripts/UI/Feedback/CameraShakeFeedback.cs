using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeFeedback : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _cm;
    [SerializeField] private float _feedbackTime = 0.2f;
    [SerializeField] private float _amplitude = 1f;
    [SerializeField] private float _frequency;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cm = FindAnyObjectByType<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        HealthSystem.OnDamaged += ShakeCamera;
    }

    private void OnDisable()
    {
        HealthSystem.OnDamaged -= ShakeCamera;

    }

    private async void ShakeCamera(int i=0)
    {
        _cm.AmplitudeGain = _amplitude;
        _cm.FrequencyGain = _frequency;

        await Awaitable.WaitForSecondsAsync(_feedbackTime);
        _cm.FrequencyGain = 0f;
        _cm.AmplitudeGain = 0f;
    }
    


}
