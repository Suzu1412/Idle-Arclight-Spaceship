using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private BoolGameEventListener OnGameplayPausedListener;

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
        if (isPaused)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
