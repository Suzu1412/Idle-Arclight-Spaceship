using System;
using UnityEngine;


public class GameInitiator : MonoBehaviour
{
    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnStartGameEvent;

    [Header("Void Event Binding")]
    [SerializeField] private VoidGameEventBinding OnFinishedLoadingEventBinding;
    private Action StartGameAction;

    private void Awake()
    {
        StartGameAction = StartGame;
    }

    private void OnEnable()
    {
        OnFinishedLoadingEventBinding.Bind(StartGameAction, this);
    }

    private void OnDisable()
    {
        OnFinishedLoadingEventBinding.Unbind(StartGameAction, this);
    }

    private void StartGame()
    {
        OnStartGameEvent.RaiseEvent(this);
    }
}
