using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyItemSO", menuName = "Scriptable Objects/Item/CurrencyItem")]
public class CurrencyItemSO : ItemSO
{
    [SerializeField] private CurrencyDataSO _currencyData;
    [SerializeField] private StringGameEvent OnPickUpCurrencyAmountEvent;
    [SerializeField][Range(1, 9999)] private double _amount;

    public override void PickUp(IAgent agent)
    {
        BigNumber amount = _currencyData.CalculateGemPickupAmount(_amount);
        _currencyData.AddCurrency(amount);
        OnPickUpCurrencyAmountEvent.RaiseEvent(amount.GetFormat(), this);
    }
}
