using System;
using UnityEngine;


public class GamePauseManager : Singleton<GamePauseManager>
{
    [SerializeField] private BoolGameEventBinding OnGameplayPausedBinding;
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private BoolVariableSO _isPaused;
    private Action<bool> PauseGameplayAction;

    protected override void Awake()
    {
        base.Awake();
        PauseGameplayAction = PauseGameplay;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDestroy()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    private void OnEnable()
    {
        OnGameplayPausedBinding.Bind(PauseGameplayAction, this);
    }

    private void OnDisable()
    {
        OnGameplayPausedBinding.Unbind(PauseGameplayAction, this);
    }

    private void PauseGameplay(bool isPaused)
    {
        Screen.sleepTimeout = isPaused ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;

        foreach (var pausable in _pausable.Items)
        {
            pausable.Pause(isPaused);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Time.timeScale = 0; // Pause game
            AudioListener.pause = true; // Pause audio
        }
        else
        {
            Time.timeScale = 1; // Resume game
            AudioListener.pause = false; // Resume audio
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Time.timeScale = 0; // Pause game
            AudioListener.pause = true; // Pause audio
        }
        else
        {
            Time.timeScale = 1; // Resume game
            AudioListener.pause = false; // Resume audio
        }
    }


}
