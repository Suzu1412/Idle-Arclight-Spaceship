using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyItemSO", menuName = "Scriptable Objects/Item/CurrencyItem")]
public class CurrencyItemSO : ItemSO
{
    [SerializeField] private DoubleGameEvent _currencyGainEvent;
    [SerializeField][Range(1, 9999)] private int _amount;

    public override void PickUp(IAgent agent)
    {
        // Pick Up By Currency Manager
        _currencyGainEvent.RaiseEvent(_amount);
    }
}
