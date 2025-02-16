using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]

[CreateAssetMenu(fileName = "CurrencyDataSO", menuName = "Scriptable Objects/CurrencyDataSO")]
public class CurrencyDataSO : ScriptableObject
{
    [Header("Currency Storage")]
    [SerializeField] private BigNumber _lifetimeCurrency;
    [SerializeField] private BigNumber _totalCurrency;

    [Header("Production Storage")]
    [SerializeField] private BigNumber _highestProduction;
    [SerializeField] private BigNumber _totalProduction;

    [Header("Base Multipliers (Permanent)")]
    [SerializeField] private float baseCurrencyMultiplier = 1f;
    [SerializeField] private float baseProductionMultiplier = 1f;
    [SerializeField] private float baseOnGetMultiplier = 1f;

    [Header("Upgrade Multipliers (Stackable)")]
    [SerializeField] private float upgradeCurrencyMultiplier = 1f;
    [SerializeField] private float upgradeProductionMultiplier = 1f;
    [SerializeField] private float upgradeOnGetMultiplier = 1f;

    [Header("Event Multipliers (Temporary)")]
    [SerializeField] private float eventCurrencyMultiplier = 1f;
    [SerializeField] private float eventProductionMultiplier = 1f;
    [SerializeField] private float eventOnGetMultiplier = 1f;

    [Header("Offline Multipliers")]
    [SerializeField] private float offlineCurrencyMultiplier = 0f;
    [SerializeField] private float offlineTimeCapMinutes = 30; // 30 minutes

    [Header("Gem Pickup Bonus")]
    [SerializeField] private float productionBonusPercentage = 0f;

    [Header("Prestige Bonus")]
    [SerializeField] private float prestigeMultiplier = 1f;

    [Header("Events")]
    [SerializeField] private VoidGameEvent OnCurrencyChangedEvent;
    [SerializeField] private VoidGameEvent OnProductionChangedEvent;


    public BigNumber LifetimeCurrency => _lifetimeCurrency;
    public BigNumber TotalCurrency => _totalCurrency;
    public BigNumber HighestProduction => _highestProduction;
    public BigNumber TotalProduction => _totalProduction;

    public float FinalCurrencyMultiplier => baseCurrencyMultiplier * upgradeCurrencyMultiplier * eventCurrencyMultiplier * prestigeMultiplier;
    public float FinalProductionMultiplier => baseProductionMultiplier * upgradeProductionMultiplier * eventProductionMultiplier;
    public float FinalOnGetMultiplier => baseOnGetMultiplier * upgradeOnGetMultiplier * eventOnGetMultiplier;

    // Setters with clamping
    public void SetBaseCurrencyMultiplier(float value)
    {
        baseCurrencyMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);
    }
    public void SetBaseProductionMultiplier(float value)
    {
        baseProductionMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);

    }

    public void SetBaseOnGetMultiplier(float value)
    {
        baseOnGetMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);

    }

    public void SetUpgradeCurrencyMultiplier(float value)
    {
        upgradeCurrencyMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);

    }

    public void SetUpgradeProductionMultiplier(float value)
    {
        upgradeProductionMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);

    }

    public void SetUpgradeOnGetMultiplier(float value)
    {
        upgradeOnGetMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);

    }

    public void SetEventCurrencyMultiplier(float value)
    {
        eventCurrencyMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);
    }

    public void SetEventProductionMultiplier(float value)
    {
        eventProductionMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);
    }

    public void SetEventOnGetMultiplier(float value)
    {
        eventOnGetMultiplier = Mathf.Max(1f, value);
        OnProductionChangedEvent.RaiseEvent(this);
    }

    public void SetPrestigeMultiplier(int prestigePoints)
    {
        prestigeMultiplier = 1f + (prestigePoints * 0.1f); // Example: Each point increases by 10%
        OnProductionChangedEvent.RaiseEvent(this);
    }

    public void ResetEventMultipliers()
    {
        eventCurrencyMultiplier = 1f;
        eventProductionMultiplier = 1f;
        eventOnGetMultiplier = 1f;

        OnProductionChangedEvent.RaiseEvent(this);
    }


    // Currency Setters
    /// <summary>
    /// Only use on Load
    /// </summary>
    /// <param name="amount"></param>
    internal void LoadCurrency(BigNumber lifetimeCurrency, BigNumber totalCurrency, BigNumber highestProduction)
    {
        _lifetimeCurrency = lifetimeCurrency;
        _highestProduction = highestProduction;
        _totalCurrency = totalCurrency;
        OnCurrencyChangedEvent.RaiseEvent(this);
    }


    public void AddCurrency(BigNumber amount)
    {
        _totalCurrency += amount;
        _lifetimeCurrency += amount;
        OnCurrencyChangedEvent.RaiseEvent(this);
    }

    public void SubtractCurrency(BigNumber amount)
    {
        _totalCurrency = BigNumber.Max(_totalCurrency - amount, BigNumber.Zero);
        OnCurrencyChangedEvent.RaiseEvent(this);

    }

    public void SetTotalProduction(BigNumber amount)
    {
        _totalProduction = amount;

        if (TotalProduction > _highestProduction)
        {
            _highestProduction = TotalProduction;
        }
    }

    public BigNumber CalculateGemProductionAmount(BigNumber production, int amount)
    {
        BigNumber gemProduction = production * amount * FinalProductionMultiplier * FinalCurrencyMultiplier;
        return gemProduction;
    }

    // Calculates gem pickup value, including multipliers and production bonus
    public BigNumber CalculateGemPickupAmount(double baseGemValue)
    {
        BigNumber totalAmount = new(baseGemValue * FinalOnGetMultiplier * FinalCurrencyMultiplier);

        // If the upgrade is active, add a percentage of current production
        totalAmount += _totalProduction * productionBonusPercentage;
        
        return totalAmount;
    }

    public BigNumber CalculateOfflineEarnings(long lastActive)
    {
        long elapsedTime = DateTime.UtcNow.Ticks - lastActive;
        double offlineSeconds = Math.Min(elapsedTime, offlineTimeCapMinutes * 60);

        if (offlineSeconds <= 0) return new BigNumber(0);

        BigNumber offlineEarnings = TotalProduction * offlineSeconds * offlineCurrencyMultiplier;
        return offlineEarnings;
    }
}
