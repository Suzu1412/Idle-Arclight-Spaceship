using UnityEngine;

public class GamePauseManager : Singleton<GamePauseManager>
{
    [SerializeField] private BoolGameEventListener OnGameplayPausedListener;
    [SerializeField] private PausableRunTimeSetSO _pausable;
    [SerializeField] private BoolVariableSO _isPaused;

    protected override void Awake()
    {
        base.Awake();
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
        OnGameplayPausedListener.Register(PauseGameplay);
    }

    private void OnDisable()
    {
        OnGameplayPausedListener.DeRegister(PauseGameplay);
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
