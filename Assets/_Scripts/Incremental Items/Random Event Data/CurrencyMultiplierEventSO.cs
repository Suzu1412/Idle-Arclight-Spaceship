using UnityEngine;
using Cysharp.Text;

[CreateAssetMenu(fileName = "CurrencyMultiplierEventSO", menuName = "Scriptable Objects/RandomEvents/CurrencyMultiplierEventSO")]
public class CurrencyMultiplierEventSO : BaseRandomEventSO
{
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;
    [SerializeField] private FloatModifier _modifier;
    [SerializeField] private VoidGameEvent OnProductionChangedEvent;

    [ContextMenu("Activate Event")]
    public override void ActivateEvent()
    {
        _crystalTotalMultiplier.AddModifier(_modifier);
        OnProductionChangedEvent.RaiseEvent();
    }

    [ContextMenu("Deactivate Event")]
    public override void DeactivateEvent()
    {
        _crystalTotalMultiplier.RemoveModifier(_modifier);
        OnProductionChangedEvent.RaiseEvent();
    }
}
