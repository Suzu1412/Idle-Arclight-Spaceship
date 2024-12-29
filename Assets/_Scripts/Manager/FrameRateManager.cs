using System.Collections;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")]
    int MaxRate = 9999;
    private int _currentFrameRate;
    private float _applicationFrameRate;

    [SerializeField] private FloatVariableSO _fpsAmount;
    [SerializeField] private FloatGameEventListener OnFPSChangeEventListener; // Listen to FPS UI

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        _applicationFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        ChangeFPSLimit(PlayerPrefs.GetFloat(key: "FPSAmount"));
    }

    private void OnEnable()
    {
        OnFPSChangeEventListener.Register(ChangeFPSLimit);
    }

    private void OnDisable()
    {
        OnFPSChangeEventListener.DeRegister(ChangeFPSLimit);
    }

    private void ChangeFPSLimit(float amount)
    {
        if (amount > 0f)
        {
            _currentFrameRate = (int)amount;
            _fpsAmount.Initialize(_currentFrameRate);
        }
        else
        {
            _currentFrameRate = (int)_applicationFrameRate;
            _fpsAmount.Initialize(0f);
        }
        Application.targetFrameRate = _currentFrameRate;
        PlayerPrefs.SetFloat("FPSAmount", _currentFrameRate);
    }
}
