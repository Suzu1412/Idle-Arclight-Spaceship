using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [HideInInspector] public string Name;
    public int CurrentSceneGroupID;

    [Header("Game Manager")]
    public GameStateType CurrentGameState;

    [Header("Players")]
    public List<PlayerAgentData> Players;

    [Header("Currency")]
    public CurrencyData CurrencyData;

    [Header("Generators")]
    public List<GeneratorData> Generators;

    [Header("Upgrades")]
    public List<UpgradeData> Upgrades;

    [Header("Unlocked Systems")]
    public List<UnlockSystemData> UnlockedSystems;

    public void NewGame(string name)
    {
        Name = name;

        CurrentSceneGroupID = 0;
        CurrentGameState = GameStateType.Init;
        Players = new();
        CurrencyData = new();
        Generators = new();
        Upgrades = new();
        UnlockedSystems = new();
    }


    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

[System.Serializable]
public class CurrencyData
{
    [SerializeField] private double _totalCurrency;
    [SerializeField] private double _gameTotalCurrency;
    [SerializeField] private int _amountToBuy = 1;
    [SerializeField] private long _lastActiveDateTime;

    public double TotalCurrency => _totalCurrency;
    public double GameTotalCurrency => _gameTotalCurrency;
    public int AmountToBuy => _amountToBuy;
    public long LastActiveDateTime => _lastActiveDateTime;

    public CurrencyData()
    {
        _totalCurrency = 0;
        _gameTotalCurrency = 0;
        _amountToBuy = 1;
        _lastActiveDateTime = DateTime.Now.Ticks;
    }

    public CurrencyData(double totalCurrency, double gameTotalCurrency, int amountToBuy)
    {
        _totalCurrency = totalCurrency;
        _gameTotalCurrency = gameTotalCurrency;
        _lastActiveDateTime = DateTime.Now.Ticks;
        if (amountToBuy < 1)
        {
            _amountToBuy = -1;
        }
        else if (amountToBuy > 100)
        {
            _amountToBuy = -1;
        }
        else
        {
            _amountToBuy = amountToBuy;
        }
    }
}

[System.Serializable]
public class GeneratorData
{
    [SerializeField] private string _guid;
    [SerializeField] private int _amount;
    [SerializeField] private double _totalProduction;
    [SerializeField] private bool _isUnlocked = false;
    [SerializeField] private bool _shouldNotify = true;

    public string Guid => _guid;
    public int Amount => _amount;
    public double TotalProduction => _totalProduction;
    public bool IsUnlocked => _isUnlocked;
    public bool ShouldNotify => _shouldNotify;

    public GeneratorData(string guid, int amount, double totalProduction, bool isUnlocked, bool shouldNotify)
    {
        _guid = guid;
        _amount = amount;
        _totalProduction = totalProduction;
        _isUnlocked = isUnlocked;
        _shouldNotify = shouldNotify;
    }
}

[System.Serializable]
public class UpgradeData
{
    [SerializeField] private string _guid;
    [SerializeField] private bool _isRequirementMet = false;
    [SerializeField] private bool _isApplied = false;

    public string Guid => _guid;
    public bool IsRequirementMet => _isRequirementMet;
    public bool IsApplied => _isApplied;

    public UpgradeData(string guid, bool isRequirementMet, bool isApplied)
    {
        _guid = guid;
        _isRequirementMet = isRequirementMet;
        _isApplied = isApplied;
    }
}

[System.Serializable]
public class PlayerAgentData
{
    [SerializeField] private string _guid;
    [SerializeField] private float _totalExp;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isUnlocked;

    public string Guid => _guid;
    public float TotalExp => _totalExp;
    public bool IsActive => _isActive;
    public bool IsUnlocked => _isUnlocked;

    public PlayerAgentData(string guid, float totalExp, bool isActive, bool isUnlocked)
    {
        _guid = guid;
        _totalExp = totalExp;
        _isActive = isActive;
        _isUnlocked = isUnlocked;
    }
}

[System.Serializable]
public class UnlockSystemData
{
    [SerializeField] private string _guid;
    [SerializeField] private bool _isUnlocked;

    public string Guid => _guid;
    public bool IsUnlocked => _isUnlocked;

    public UnlockSystemData(string guid, bool isUnlocked)
    {
        _guid = guid;
        _isUnlocked = isUnlocked;
    }
}