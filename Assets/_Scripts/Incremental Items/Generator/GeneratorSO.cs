using UnityEngine;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/Incremental/Generator/GeneratorSO")]
public class GeneratorSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private int _amountOwned;

    [Header("Cost")]
    [SerializeField] protected double _cost;
    [SerializeField] protected double _costRequirement;
    [SerializeField] private FloatVariableSO _gemCostMultiplier;
    [SerializeField] private double _priceGrowthRate;

    [Header("Production")]
    [SerializeField] protected double _baseProduction;
    [Header("Apply Multipliers to current gem")]
    [SerializeField] private FloatVariableSO _gemProductionMultiplier;
    [Header("Random Event Production Multiplier")]
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [Header("Random Event Total Multiplier")]
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;


    [Header("Notification")]
    [SerializeField] private NotificationGameEvent OnShopNotificationEvent;
    [SerializeField] private Sprite _notificationIcon;
    private NotificationSO _notification;


    private DoubleVariableSO _bulkCost;
    private DoubleVariableSO _currentProduction;
    private DoubleVariableSO _totalProduction;
    private DoubleVariableSO _production;


    [SerializeField] private bool _isUnlocked;
    private bool _isDirty = true;

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost { get => _cost; internal set => _cost = value; }
    public double BaseProduction { get => _baseProduction; internal set => _baseProduction = value; }
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public double PriceGrowthRate { get => _priceGrowthRate; internal set => _priceGrowthRate = value; }
    public double TotalProduction { get => _totalProduction.Value; internal set => SetTotalProduction(value); }
    public double CostRequirement => _costRequirement;
    public DoubleVariableSO Cost => _bulkCost;
    public DoubleVariableSO Production => _production;
    public bool IsUnlocked { get => _isUnlocked; internal set => _isUnlocked = value; }
    public FormattedNumber CostFormatted { get; private set; }
    public FormattedNumber ProductionFormatted { get; private set; }

    private void OnEnable()
    {
        _bulkCost = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _bulkCost.Initialize(0, 0, double.MaxValue);

        _currentProduction = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _currentProduction.Initialize(0, 0, double.MaxValue);

        _production = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _production.Initialize(0, 0, double.MaxValue);

        _totalProduction = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _totalProduction.Initialize(0, 0, double.MaxValue);

        _notification = ScriptableObject.CreateInstance<NotificationSO>();
    }

    internal void Initialize()
    {
        SetAmount(0);
        SetTotalProduction(0);
        _isUnlocked = false;
    }

    public void CheckIfMeetRequirementsToUnlock(double currency)
    {
        if (currency >= CostRequirement)
        {
            _isUnlocked = true;
        }
    }
    public void AddAmount(int amount)
    {
        _isDirty = true;
        _amountOwned += amount;
    }

    public void CalculateProductionRate()
    {
        _production.Value = _baseProduction * _gemProductionMultiplier.Value * _generatorProductionMultiplier.Value * _crystalTotalMultiplier.Value;
        _currentProduction.Value = Math.Round(_production.Value * _amountOwned, 1);
        ProductionFormatted = FormatNumber.FormatDouble(_currentProduction.Value, ProductionFormatted);
        _isDirty = false;
    }

    public double GetProductionRate()
    {
        if (_isDirty)
        {
            CalculateProductionRate();
        }
        TotalProduction = Math.Round(TotalProduction + _currentProduction.Value, 1);
        return _currentProduction.Value;
    }

    public double GetBulkCost(int amountTobuy)
    {
        _bulkCost.Value = 0;

        for (int i = 0; i < amountTobuy; i++)
        {
            _bulkCost.Value += GetNextCost(i);
        }

        CostFormatted = FormatNumber.FormatDouble(_bulkCost.Value, CostFormatted);
        return _bulkCost.Value;
    }

    public int CalculateMaxAmountToBuy(double currency)
    {
        int amountToBuy = 0;
        double currencyLeft = currency;
        bool hasEnoughCurrency = true;

        while (hasEnoughCurrency)
        {
            if (currencyLeft >= GetNextCost(amountToBuy))
            {
                currencyLeft -= GetNextCost(amountToBuy);
                amountToBuy++;
            }
            else
            {
                hasEnoughCurrency = false;
            }
        }

        return amountToBuy;
    }

    internal void SetAmount(int amount)
    {
        _isDirty = true;
        _amountOwned = amount;
    }

    internal void SetTotalProduction(double totalProduction)
    {
        _totalProduction.Value = totalProduction;
    }

    internal double GetNextCost(int addAmount = 0)
    {
        return Math.Ceiling(_cost * _gemCostMultiplier.Value * Math.Pow(_priceGrowthRate, _amountOwned + addAmount));
    }

    internal void AddModifier(FloatModifier modifier)
    {
        _gemProductionMultiplier.AddModifier(modifier);
    }

    internal void RemoveModifier(FloatModifier modifier)
    {
        _gemProductionMultiplier.RemoveModifier(modifier);
    }
    private void Notificate()
    {
        _notification.SetMessage("newGeneratorNotification");
        _notification.SetSprite(_notificationIcon);
        OnShopNotificationEvent.RaiseEvent(_notification);

    }
}
