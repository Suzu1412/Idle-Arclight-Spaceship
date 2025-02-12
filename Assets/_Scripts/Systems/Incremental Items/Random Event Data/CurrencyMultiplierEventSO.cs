using UnityEngine;
using Cysharp.Text;

[CreateAssetMenu(fileName = "CurrencyMultiplierEventSO", menuName = "Scriptable Objects/Incremental/RandomEvents/CurrencyMultiplierEventSO")]
public class CurrencyMultiplierEventSO : BaseRandomEventSO
{
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;
    [SerializeField] private FloatModifier _modifier;
    [SerializeField] private VoidGameEvent OnProductionChangedEvent;

    public override float Multiplier => _modifier.Value;

    public override void ActivateEvent()
    {
        _crystalTotalMultiplier.AddModifier(_modifier);
        OnProductionChangedEvent.RaiseEvent(this);
    }

    public override void DeactivateEvent()
    {
        _crystalTotalMultiplier.RemoveModifier(_modifier);
        OnProductionChangedEvent.RaiseEvent(this);
    }
}
