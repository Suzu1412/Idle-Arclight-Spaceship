using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private BoolGameEventListener OnGameplayPausedListener;
    [SerializeField] private PausableRunTimeSetSO _pausable;

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
}
