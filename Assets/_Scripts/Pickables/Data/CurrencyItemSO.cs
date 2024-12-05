using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyItemSO", menuName = "Scriptable Objects/Item/CurrencyItem")]
public class CurrencyItemSO : ItemSO
{
    [Header("Gem Multiplier")]
    [SerializeField] private DoubleGameEvent _currencyGainEvent;
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;

    [Header("Gem Production Multiplier")]
    [SerializeField] private DoubleVariableSO _currentProduction;


    [SerializeField][Range(1, 9999)] private double _amount;

    public override void PickUp(IAgent agent)
    {
        // Pick Up By Currency Manager
        double amount = _amount;
        amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value;
        _currencyGainEvent.RaiseEvent(amount);
    }
}
