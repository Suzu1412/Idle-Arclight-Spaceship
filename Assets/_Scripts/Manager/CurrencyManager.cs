using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    [SerializeField] private double _totalAmount;
    [SerializeField] private IntGameEventListener _gainCurrencyListener;

    private void OnEnable()
    {
        _gainCurrencyListener.Register(Increment);
    }

    private void OnDisable()
    {
        _gainCurrencyListener.DeRegister(Increment);
    }

    private void Increment(int amount)
    {
        _totalAmount += amount;
    }

}
