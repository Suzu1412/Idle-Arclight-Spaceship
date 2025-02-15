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
    [SerializeField] private FloatVariableSO _productionPercentage;

    [Header("Gem Amount Multiplier")]
    [SerializeField] private DoubleVariableSO _gemTotalAmount;
    [SerializeField] private FloatVariableSO _gemTotalAmountMultiplier;
    [SerializeField] private StringGameEvent OnCurrencyGainEvent;



    [SerializeField][Range(1, 9999)] private double _amount;

    public override void PickUp(IAgent agent)
    {


        //double amount = _amount;
        //amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value + (_currentProduction.Value * _productionPercentage.Value);
        //amount += amount * (_gemTotalAmount.Value * _gemTotalAmountMultiplier.Value);

        // Pick Up By Currency Manager
        //OnCurrencyGainEvent.
        //_currencyGainEvent.RaiseEvent(amount, this);
    }
}
