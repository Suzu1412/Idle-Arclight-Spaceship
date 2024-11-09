using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyMultiplierEventSO", menuName = "Scriptable Objects/RandomEvents/CurrencyMultiplierEventSO")]
public class CurrencyMultiplierEventSO : BaseRandomEventSO
{
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;
    [SerializeField] private FloatModifier _modifier;

    [ContextMenu("Activate Event")]
    public override void ActivateEvent()
    {
        _crystalTotalMultiplier.AddModifier(_modifier);
        Debug.Log($"Activate Currency Total Multiplier Event: {_modifier.Value}");
    }

    [ContextMenu("Deactivate Event")]
    public override void DeactivateEvent()
    {
        _crystalTotalMultiplier.RemoveModifier(_modifier);
        Debug.Log($"Deactivate Currency Total Multiplier Event: {_modifier.Value}");
    }
}
