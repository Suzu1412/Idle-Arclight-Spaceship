using UnityEngine;

public class RandomEventManager : Singleton<RandomEventManager>
{
    [Header("Float Variable")]
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;

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

    private void ActivateRandomEvent()
    {

    }


}
