using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.U2D;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/Incremental/Generator/GeneratorSO")]
public class GeneratorSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private string _imageName;
    [SerializeField] private int _amountOwned;

    [Header("Cost")]
    [SerializeField] protected BigNumber _costBigNumber;
    [SerializeField] protected double _cost;
    [SerializeField] protected double _costRequirement;
    [SerializeField] private FloatVariableSO _gemCostMultiplier;
    [SerializeField] private double _priceGrowthRate;
    [SerializeField] private SerializedDictionary<int, BigNumber> _costCache = new SerializedDictionary<int, BigNumber>();

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

    private BigNumber _bulkCostBigNumber;

    private DoubleVariableSO _bulkCost;
    private DoubleVariableSO _currentProduction;
    private DoubleVariableSO _totalProduction;
    private DoubleVariableSO _production;
    private NotificationSO _notification;

    [SerializeField] private bool _isUnlocked;
    [SerializeField] private bool _shouldNotify;
    [SerializeField] private float _productionPercentage;
    private double _logPriceGrowthRate;
    private BigNumber _cachedGrowthFactor;
    private int _lastAmountOwned;
    [SerializeField] private string _costFormatted => _costBigNumber.GetFormat();
    private bool _isDirty = true;

    public string Name => _name;
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
    public bool IsDirty { set => _isDirty = value; }

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
        _logPriceGrowthRate = Math.Log(_priceGrowthRate);
        ClearCache();

    }

    internal void Initialize()
    {
        SetAmount(0);
        SetTotalProduction(0);
        _productionPercentage = 0;
        _isUnlocked = false;
        _shouldNotify = true;
        _lastAmountOwned = 0;
    }

    public void CheckIfMeetRequirementsToUnlock(double currency)
    {
        if (_isUnlocked) return;
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
        ClearCache();
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
        if (_generatorsTotalProduction.Value != 0) // Avoid Division by zero
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

    public BigNumber GetBulkCost(int amountToBuy = 1)
    {
        if (amountToBuy <= 0) return BigNumber.Zero;

        int key = _amountOwned + amountToBuy;

        if (_costCache.TryGetValue(key, out BigNumber cachedCost))
        {
            _bulkCostBigNumber = cachedCost;
            return _bulkCostBigNumber;
        }

        _bulkCostBigNumber = BigNumber.Zero;

        BigNumber firstCost = GetNextCost();

        if (_priceGrowthRate == 1)
        {
            // If growth rate is 1, the cost remains constant, so we just multiply
            _bulkCostBigNumber = firstCost * amountToBuy;
        }
        else
        {
            // Use the geometric series sum formula
            _bulkCostBigNumber = firstCost * (BigNumber.Pow(_priceGrowthRate, amountToBuy) - BigNumber.One) / (_priceGrowthRate - 1);
        }

        _costCache[key] = _bulkCostBigNumber;
        return _bulkCostBigNumber;
    }

    public int CalculateMaxAmountToBuy(BigNumber currency)
    {
        if (currency <= BigNumber.Zero) return 0;

        BigNumber firstCost = GetNextCost();

        if (_priceGrowthRate == 1)
        {
            return (currency / firstCost).ToInt();
        }

        // Optimized logarithm-based formula to solve for n
        double maxPurchases = (BigNumber.Log(currency * (_priceGrowthRate - 1) + firstCost) - BigNumber.Log(firstCost)) / _logPriceGrowthRate;

        return Math.Max(0, (int)Math.Floor(maxPurchases));
    }

    public Sprite GetSprite()
    {
        return _spriteAtlas.GetSprite(_imageName);
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

    internal BigNumber GetNextCost()
    {
        if (_priceGrowthRate == 1) return (_costBigNumber * _gemCostMultiplier.Value).Ceil();

        if (_amountOwned != _lastAmountOwned)
        {
            _cachedGrowthFactor = BigNumber.Pow(_priceGrowthRate, _amountOwned);
            _lastAmountOwned = _amountOwned;
        }

        return (_costBigNumber * _gemCostMultiplier.Value * _cachedGrowthFactor).Ceil();
    }

    internal void AddModifier(FloatModifier modifier)
    {
        _gemProductionMultiplier.AddModifier(modifier);
    }

    internal void RemoveModifier(FloatModifier modifier)
    {
        _gemProductionMultiplier.RemoveModifier(modifier);
    }

    // Call this whenever `_amountOwned` changes to clear outdated cache
    void ClearCache()
    {
        _costCache.Clear();
    }

    [ContextMenu("Convert Cost to Big Number")]
    private void CalculateCostToBigNumber()
    {
        Debug.Log(new BigNumber(_cost));
    }

    [ContextMenu("Convert Requirement to Big Number")]
    private void CalculateRequirementToBigNumber()
    {
        Debug.Log(new BigNumber(_costRequirement));
    }

    [ContextMenu("Convert Production to Big Number")]
    private void CalculateProductionToBigNumber()
    {
        Debug.Log(new BigNumber(_baseProduction));
    }

    private void Notificate()
    {
        _notification.SetMessage("newGeneratorNotification");
        _notification.SetSprite(_notificationIcon);
        OnShopNotificationEvent.RaiseEvent(_notification, this);
    }
}
