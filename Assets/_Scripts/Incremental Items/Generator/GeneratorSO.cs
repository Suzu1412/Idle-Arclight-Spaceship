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
    [SerializeField] private Sprite _notificationIcon;
    [SerializeField] private NotificationGameEvent OnShopNotificationEvent;
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _generatorsTotalProduction;

    private DoubleVariableSO _bulkCost;
    private DoubleVariableSO _currentProduction;
    private DoubleVariableSO _totalProduction;
    private DoubleVariableSO _production;
    private NotificationSO _notification;

    [SerializeField] private bool _isUnlocked;
    [SerializeField] private bool _shouldNotify;
    [SerializeField] private float _productionPercentage;
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
    public bool ShouldNotify { get => _shouldNotify; set => _shouldNotify = value; }
    public float ProductionPercentage => _productionPercentage;
    public bool IsDirty { set  => _isDirty = value; }

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
        _productionPercentage = 0;
        _isUnlocked = false;
        _shouldNotify = true;
    }

    public void CheckIfMeetRequirementsToUnlock(double currency)
    {
        if (currency >= CostRequirement)
        {
            _isUnlocked = true;
            Notificate();
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

    public void CalculatePercentage()
    {
        if (_generatorsTotalProduction.Value != 0) // Evita división por Cero
        {
            _productionPercentage = (float)(_currentProduction.Value / _generatorsTotalProduction.Value) * 100;
        }
        else
        {
            _productionPercentage = 0;
        }
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

    public double GetBulkCost(int amountTobuy = 1)
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

    [ContextMenu("Show Bulk Cost")]
    private void CalculateBulkCost()
    {
        Debug.Log($"Generator Name: {this.Name}");
        SetAmount(9);
        Debug.Log($"10 Bulk Cost: {GetBulkCost()}");
        SetAmount(24);
        Debug.Log($"25 Bulk Cost: {GetBulkCost()}");
        SetAmount(49);
        Debug.Log($"50 Bulk Cost: {GetBulkCost()}");
        SetAmount(99);
        Debug.Log($"100 Bulk Cost: {GetBulkCost()}");
        SetAmount(149);
        Debug.Log($"150 Bulk Cost: {GetBulkCost()}");
        SetAmount(199);
        Debug.Log($"200 Bulk Cost: {GetBulkCost()}");
        SetAmount(249);
        Debug.Log($"250 Bulk Cost: {GetBulkCost()}");
        SetAmount(299);
        Debug.Log($"300 Bulk Cost: {GetBulkCost()}");
        SetAmount(349);
        Debug.Log($"350 Bulk Cost: {GetBulkCost()}");
        SetAmount(399);
        Debug.Log($"400 Bulk Cost: {GetBulkCost()}");
        SetAmount(449);
        Debug.Log($"450 Bulk Cost: {GetBulkCost()}");
        SetAmount(499);
        Debug.Log($"500 Bulk Cost: {GetBulkCost()}");
    }
}
