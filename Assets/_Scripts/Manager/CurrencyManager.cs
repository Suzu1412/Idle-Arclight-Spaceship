using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;

    [SerializeField] private CurrencyDataSO _currencyData;

    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnUpgradeBoughtEvent;

    [Header("Void Event Binding")]
    [SerializeField] private VoidGameEventBinding OnBuyEveryUpgradeEventBinding;
    [Header("Int Event Listener")]

    [SerializeField] private IntGameEventListener OnBuyUpgradeGameEventListener;

    [SerializeField] private ListUpgradeSO _upgrades;

    // Actions
    private Action BuyEveryUpgradeAvailableAction;


    protected override void Awake()
    {
        base.Awake();
        BuyEveryUpgradeAvailableAction = BuyEveryUpgradeAvailable;
    }

    private void OnEnable()
    {
        _saveable.Add(this);
        OnBuyUpgradeGameEventListener.Register(BuyUpgrade);
        OnBuyEveryUpgradeEventBinding.Bind(BuyEveryUpgradeAvailableAction, this);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnBuyUpgradeGameEventListener.DeRegister(BuyUpgrade);
        OnBuyEveryUpgradeEventBinding.Unbind(BuyEveryUpgradeAvailableAction, this);
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.CurrencyData = new(_currencyData.LifetimeCurrency, _currencyData.TotalCurrency, _currencyData.TotalProduction);

        gameData.Upgrades.Clear();
        List<UpgradeData> upgradeDatas = gameData.Upgrades;
        foreach (var upgrade in _upgrades.Upgrades)
        {
            upgradeDatas.Add(new(upgrade.Guid, upgrade.IsRequirementMet, upgrade.IsApplied));
        }

        gameData.Upgrades = upgradeDatas;
    }

    public void LoadData(GameDataSO gameData)
    {
        LoadCurrency(gameData.CurrencyData);
        LoadUpgrades(gameData.Upgrades);
        //LoadOfflineReward(gameData.CurrencyData.LastActiveDateTime);

        OnUpgradeBoughtEvent.RaiseEvent(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void BuyUpgrade(int index)
    {
        //if (_currencyData.TotalCurrency.Value >= _upgrades.Upgrades[index].Cost.Value)
        //{
        //    _upgrades.Upgrades[index].BuyUpgrade(_totalCurrency.Value);
        //    _totalCurrency.Value -= _upgrades.Upgrades[index].Cost.Value;
        //    GetProductionRate();
        //    OnUpgradeBoughtEvent.RaiseEvent(this);
        //    OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted), this);
        //    UpdateCurrency();

        //}
    }

    private void BuyEveryUpgradeAvailable()
    {
        int upgradesBought = 0;

        //for (int i = 0; i < _upgrades.Upgrades.Count; i++)
        //{
        //    if (!_upgrades.Upgrades[i].IsRequirementMet) continue;
        //    if (_upgrades.Upgrades[i].IsApplied) continue;
        //    if (_totalCurrency.Value < _upgrades.Upgrades[i].Cost.Value) continue;

        //    BuyUpgrade(i);
        //    upgradesBought++;
        //}

        Debug.Log($"Has comprado {upgradesBought} mejoras");
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.WaitOneSecond;
            _currencyData.AddCurrency(_currencyData.TotalProduction);
        }
    }

    private void LoadCurrency(CurrencyData currencyData)
    {
        _currencyData.LoadCurrency(currencyData.LifetimeCurrency, currencyData.TotalCurrency, currencyData.HighestProduction);
    }

    private void LoadUpgrades(List<UpgradeData> upgradeDatas)
    {
        foreach (var upgradeSO in _upgrades.Upgrades)
        {
            upgradeSO.Initialize();
        }

        foreach (var upgrade in upgradeDatas)
        {
            var upgradeSO = _upgrades.Find(upgrade.Guid);
            if (upgradeSO == null) return;
            upgradeSO.IsRequirementMet = upgrade.IsRequirementMet;
            upgradeSO.ApplyUpgrade(upgrade.IsApplied);
        }
    }
}
