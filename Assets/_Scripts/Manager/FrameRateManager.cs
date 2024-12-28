using System.Collections;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;
    [Header("Frame Settings")]
    int MaxRate = 9999;
    [SerializeField] private float TargetFrameRate = 30f;
    private int _currentFrameRate;
    private float _applicationFrameRate;
    float currentFrameTime;

    [SerializeField] private FloatVariableSO _fpsAmount;
    [SerializeField] private FloatGameEventListener OnFPSChangeEventListener; // Listen to FPS UI

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        _applicationFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        //StartCoroutine(WaitForNextFrame());
    }

    private void OnEnable()
    {
        _saveable.Add(this);
        OnFPSChangeEventListener.Register(ChangeFPSLimit);
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnFPSChangeEventListener.DeRegister(ChangeFPSLimit);
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
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
        TargetFrameRate = _currentFrameRate;
        Application.targetFrameRate = _currentFrameRate;

    }

    public void SaveData(GameDataSO gameData)
    {
        PlayerPrefs.SetFloat("FPSAmount", _currentFrameRate);

    }

    public void LoadData(GameDataSO gameData)
    {
        ChangeFPSLimit(PlayerPrefs.GetFloat("FPSAmount"));
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
