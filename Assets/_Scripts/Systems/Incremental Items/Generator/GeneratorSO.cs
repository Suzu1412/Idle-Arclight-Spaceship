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
    [Header("Generator Data")]
    [SerializeField] private string _name;
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private string _imageName;
    [SerializeField] private int _amountOwned;
    [SerializeField] private CurrencyDataSO _currencyData;


    [Header("Cost")]
    [SerializeField] private BigNumber _baseCost;
    [SerializeField] private BigNumber _storeRevealCost;
    [SerializeField][ReadOnly] private BigNumber _bulkCost;
    [SerializeField] private double _priceGrowthRate;


    [Header("Production")]
    [SerializeField] private BigNumber _baseProduction;
    [SerializeField] private BigNumber _production;
    [SerializeField] private BigNumber _totalProduction;
    [SerializeField] private FloatVariableSO _gemProductionMultiplier;


    [SerializeField] private SerializedDictionary<int, BigNumber> _costCache = new SerializedDictionary<int, BigNumber>();

    [Header("Apply Multipliers to current gem")]



    [SerializeField] private bool _isVisibleInStore;
    [SerializeField] private float _productionPercentage;
    private double _logPriceGrowthRate;
    private BigNumber _cachedGrowthFactor;
    private int _lastAmountOwned;
    private bool _hasProductionChanged = true;

    public string Name => _name;
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public BigNumber BulkCost => _bulkCost;
    public BigNumber Production => _production;
    public BigNumber TotalProduction => _totalProduction;
    public BigNumber StoreRevealCost => _storeRevealCost;
    public bool IsVisibleInStore { get => _isVisibleInStore; internal set => _isVisibleInStore = value; }
    public float ProductionPercentage => _productionPercentage;
    public bool HasProductionChanged { set => _hasProductionChanged = value; }

    private void OnEnable()
    {
        _logPriceGrowthRate = Math.Log(_priceGrowthRate);
        ClearCache();
    }

    internal void Initialize()
    {
        SetAmount(0);
        SetTotalProduction(BigNumber.Zero);
        _productionPercentage = 0;
        _isVisibleInStore = false;
        _lastAmountOwned = 0;
    }

    public void CheckIfMeetRequirementsToUnlock(BigNumber currency)
    {
        if (_isVisibleInStore) return;
        if (currency >= StoreRevealCost)
        {
            _isVisibleInStore = true;
            //Notificate();
        }
    }
    public void AddAmount(int amount)
    {
        _hasProductionChanged = true;
        _amountOwned += amount;
        ClearCache();
    }

    public void CalculateProductionRate()
    {
        _production = _currencyData.CalculateGemProductionAmount(_baseProduction * _gemProductionMultiplier.Value, _amountOwned);

        _hasProductionChanged = false;
    }

    public void CalculatePercentage(BigNumber totalProduction)
    {
        if (totalProduction != BigNumber.Zero) // Avoid Division by zero
        {
            _productionPercentage = (_production / totalProduction * 100).ToFloat();
        }
        else
        {
            _productionPercentage = 0;
        }
    }

    public BigNumber GetProductionRate()
    {
        if (_hasProductionChanged)
        {
            CalculateProductionRate();
        }
        _totalProduction += _production;
        return _production;
    }

    public BigNumber GetBulkCost(int amountToBuy = 1)
    {
        if (amountToBuy <= 0) return BigNumber.Zero;

        int key = _amountOwned + amountToBuy;

        if (_costCache.TryGetValue(key, out BigNumber cachedCost))
        {
            _bulkCost = cachedCost;
            return _bulkCost;
        }

        _bulkCost = BigNumber.Zero;

        BigNumber firstCost = GetNextCost();

        if (_priceGrowthRate == 1)
        {
            // If growth rate is 1, the cost remains constant, so we just multiply
            _bulkCost = firstCost * amountToBuy;
        }
        else
        {
            // Use the geometric series sum formula
            _bulkCost = firstCost * (BigNumber.Pow(_priceGrowthRate, amountToBuy) - BigNumber.One) / (_priceGrowthRate - 1);
        }

        _costCache[key] = _bulkCost;
        return _bulkCost;
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
        _hasProductionChanged = true;
        _amountOwned = amount;
    }

    internal void SetTotalProduction(BigNumber totalProduction)
    {
        _totalProduction = totalProduction;
    }

    internal BigNumber GetNextCost()
    {
        if (_priceGrowthRate == 1) return _baseCost.Ceil(); // We can add a: _baseCost * _gemCostMultiplier.Value in case of something that modifies the price

        if (_amountOwned != _lastAmountOwned)
        {
            _cachedGrowthFactor = BigNumber.Pow(_priceGrowthRate, _amountOwned);
            _lastAmountOwned = _amountOwned;
        }

        return (_baseCost * _cachedGrowthFactor).Ceil(); // we can also add the multiplier here
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
}
