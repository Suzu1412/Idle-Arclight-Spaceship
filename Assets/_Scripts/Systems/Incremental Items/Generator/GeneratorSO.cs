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
    [SerializeField] internal int _amountOwned;
    [SerializeField] private CurrencyDataSO _currencyData;


    [Header("Cost")]
    [SerializeField] internal BigNumber _baseCost;
    [SerializeField] private BigNumber _storeRevealCost;
    [SerializeField][ReadOnly] private BigNumber _bulkCost;
    [SerializeField] internal double _priceGrowthRate;


    [Header("Production")]
    [SerializeField] internal BigNumber _baseProduction;
    [SerializeField][ReadOnly] private BigNumber _production;
    [SerializeField][ReadOnly] private BigNumber _totalProduction;
    [SerializeField] private FloatVariableSO _gemProductionMultiplier;


    [SerializeField] private SerializedDictionary<int, BigNumber> _costCache = new SerializedDictionary<int, BigNumber>();

    [SerializeField] private bool _isVisibleInStore;
    [SerializeField] private float _productionPercentage;
    private double _logPriceGrowthRate;
    private double _cachedGrowthFactor;
    private int _lastAmountOwned;
    private bool _hasProductionChanged = true;

    public string Name => _name;
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public BigNumber BulkCost => _bulkCost;
    public BigNumber Production => _production;
    public BigNumber TotalProduction => _totalProduction;
    public BigNumber StoreRevealCost => _storeRevealCost;
    public BigNumber BaseProduction => _baseProduction * _gemProductionMultiplier.Value;
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
        _lastAmountOwned = -1;
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
        _production = _currencyData.CalculateGemProductionAmount(_baseProduction, _amountOwned, _gemProductionMultiplier.Value);

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

        BigNumber firstCost = GetTotalCostForGenerators(1);
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

    public int GetMaxGenerators(BigNumber totalCurrency)
    {
        int low = 0;
        int high = 10000; // Large number; adjust as needed
        int best = 0;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            BigNumber cost = GetTotalCostForGenerators(mid);
            
            if (cost <= totalCurrency)
            {
                best = mid; // We can afford this many, try more
                low = mid + 1;
            }
            else
            {
                high = mid - 1; // Too expensive, try fewer
            }
            
        }

        return best;
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

    internal BigNumber GetTotalCostForGenerators(int n)
    {
        if (_priceGrowthRate == 1) return _baseCost * n; // Linear growth case

        if (_amountOwned != _lastAmountOwned || _cachedGrowthFactor == 0)
        {
            _cachedGrowthFactor = Math.Pow(_priceGrowthRate, _amountOwned);
            _lastAmountOwned = _amountOwned;
        }

        BigNumber firstCost = _baseCost * _cachedGrowthFactor; // Cost of next generator

        // Geometric series sum formula: S = a * (1 - r^n) / (1 - r)
        BigNumber growthFactor = BigNumber.Pow(_priceGrowthRate, n);
        return firstCost * ((BigNumber.One - growthFactor) / (BigNumber.One - _priceGrowthRate));
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
