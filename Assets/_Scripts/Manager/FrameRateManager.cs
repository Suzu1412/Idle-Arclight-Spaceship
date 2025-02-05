using System.Collections;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private bool isMobile;

    [SerializeField] private IntVariableSO _targetFramerate;
    [SerializeField] private IntGameEventListener OnFPSChangeEventListener; // Listen to FPS UI

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
    }

    void Start()
    {
        DeterminePlatform();
        int fpsAmount = PlayerPrefs.GetInt(key: "FPSAmount");

        if (fpsAmount <= 0 && isMobile)
        {
            fpsAmount = 60;
        }
        else if (fpsAmount <= 0 && !isMobile)
        {
            fpsAmount = -1;
        }

        SetFrameRate(fpsAmount);
    }

    private void OnEnable()
    {
        OnFPSChangeEventListener.Register(SetFrameRate);
    }

    private void OnDisable()
    {
        OnFPSChangeEventListener.DeRegister(SetFrameRate);
    }

    public void SetFrameRate(int targetFrameRate)
    {
        _targetFramerate.Value = targetFrameRate;
        Application.targetFrameRate = _targetFramerate.Value;
        AdjustPhysicsTimestep(_targetFramerate.Value);

        // Disable VSync if using Application.targetFrameRate
        QualitySettings.vSyncCount = (_targetFramerate.Value == -1) ? 1 : 0; // Enable VSync for unlimited FPS
        Debug.Log($"Frame rate set to: {(targetFrameRate == -1 ? "Unlimited" : targetFrameRate)}");
        PlayerPrefs.SetInt("FPSAmount", _targetFramerate.Value);
    }

    private void AdjustPhysicsTimestep(int targetFrameRate)
    {
        if (targetFrameRate > 0) // Adjust Fixed Timestep for specific frame rates
        {
            Time.fixedDeltaTime = 1f / targetFrameRate; // Match the physics calculation rate
            //Debug.Log($"Fixed Timestep adjusted to {Time.fixedDeltaTime} for {targetFrameRate} FPS");
        }
        else
        {
            Time.fixedDeltaTime = 0.02f; // Default Fixed Timestep for unlimited FPS
            //Debug.Log("Fixed Timestep set to default (0.02) for unlimited FPS");
        }
    }

    private void DeterminePlatform()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Application.targetFrameRate = 5;// Pause game
        }
        else
        {
            Application.targetFrameRate = _targetFramerate.Value; // Resume game
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Application.targetFrameRate = 5; // Pause game
        }
        else
        {
            Application.targetFrameRate = _targetFramerate.Value;  // Resume game
        }
    }
}
