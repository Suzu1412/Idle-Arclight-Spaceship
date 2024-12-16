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

    [Header("Language")]
    public LocalizationData LocalizationData;

    [Header("Target Volume")]
    public VolumeData VolumeData;

    [Header("Target FPS")]
    public FPSData FPSData;


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
        LocalizationData = new();
        VolumeData = new();
        FPSData = new();
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
            _amountToBuy = 1;
        }
        else if (amountToBuy > 100)
        {
            _amountToBuy = 100;
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

    public string Guid => _guid;
    public int Amount => _amount;
    public double TotalProduction => _totalProduction;
    public bool IsUnlocked => _isUnlocked;

    public GeneratorData(string guid, int amount, double totalProduction, bool isUnlocked)
    {
        _guid = guid;
        _amount = amount;
        _totalProduction = totalProduction;
        _isUnlocked = isUnlocked;
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

[System.Serializable]
public class VolumeData
{
    [SerializeField] private float _masterVolume;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _sfxVolume;

    public float MasterVolume => _masterVolume;
    public float MusicVolume => _musicVolume;
    public float SFXVolume => _sfxVolume;

    public VolumeData()
    {
        _masterVolume = 1f;
        _musicVolume = 1f;
        _sfxVolume = 1f;
    }

    public VolumeData(float masterVolume, float musicVolume, float sfxVolume)
    {
        _masterVolume = masterVolume;
        _musicVolume = musicVolume;
        _sfxVolume = sfxVolume;
    }

}

[System.Serializable]
public class LocalizationData
{
    [SerializeField] private string _locale;

    public string Locale => _locale;

    public LocalizationData()
    {
        _locale = null;
    }

    public LocalizationData(string locale)
    {
        _locale = locale;
    }
}

[System.Serializable]
public class FPSData
{
    [SerializeField] private int _fpsAmount;

    public int FPSAmount => _fpsAmount;

    public FPSData()
    {
        _fpsAmount = 0;
    }

    public FPSData(int fpsAmount)
    {
        _fpsAmount = fpsAmount;
    }
}