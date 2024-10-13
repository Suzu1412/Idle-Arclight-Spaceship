using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private DoubleGameEventListener _gainCurrencyListener;
    [SerializeField] private DoubleGameEvent _totalCurrency;
    [Header("Save Data")]
    [SerializeField] private CurrencyData _currencyData;
    [field: SerializeField] public SerializableGuid Id { get; set; }

    protected override void Awake()
    {
        base.Awake();
        _currencyData = new CurrencyData();
    }

    private void OnEnable()
    {
        _gainCurrencyListener.Register(Increment);
    }

    private void OnDisable()
    {
        _gainCurrencyListener.DeRegister(Increment);
    }

    private void Increment(double amount)
    {
        _currencyData.TotalCurrency += amount;
        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }

    public void SaveData(GameData gameData)
    {
        gameData.CurrencyData = _currencyData;
    }

    public void LoadData(GameData gameData)
    {
        _currencyData.TotalCurrency = gameData.CurrencyData.TotalCurrency;
        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }
}
