using UnityEngine;


public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnStartGameEvent;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnFinishedLoadingEventListener;

    private void OnEnable()
    {
        OnFinishedLoadingEventListener.Register(StartGame);
    }

    private void OnDisable()
    {
        OnFinishedLoadingEventListener.DeRegister(StartGame);
    }

    private void StartGame()
    {
        OnStartGameEvent.RaiseEvent();
    }
}
