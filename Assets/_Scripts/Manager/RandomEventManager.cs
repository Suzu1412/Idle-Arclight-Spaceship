using UnityEngine;

public class RandomEventManager : Singleton<RandomEventManager>
{
    [SerializeField] private RandomEventsSO _randomEvents;

    [Header("Float Variable")]
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;
    [SerializeField] private FloatVariableSO _expMultiplier;


    [Header("Void Event")]
    [SerializeField] private VoidGameEventListener OnActivateRandomEventListener;

    private void OnEnable()
    {
        OnActivateRandomEventListener.Register(ActivateRandomEvent);
    }

    private void OnDisable()
    {
        OnActivateRandomEventListener.DeRegister(ActivateRandomEvent);
    }

    private void Start()
    {
        ActivateRandomEvent();
    }

    private void ActivateRandomEvent()
    {
        foreach(var randomEvent in _randomEvents.RandomEvents)
        {
            randomEvent.ActivateEvent();
        }
    }


}
