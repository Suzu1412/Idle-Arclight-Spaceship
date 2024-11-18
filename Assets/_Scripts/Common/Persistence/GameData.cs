using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string Name;

    public string LocationId;
    public string CurrentLevelName;

    [Header("Game Manager")]
    public GameStateType CurrentGameState;

    [Header("Players")]
    public PlayerAgentData CurrentPlayerData;
    public PlayerAgentDatas PlayerAgentDatas = new();

    [Header("Currency")]
    public CurrencyData CurrencyData = new();

    [Header("Generators")]
    public GeneratorsData GeneratorsData = new();

    [Header("Upgrades")]
    public UpgradesData UpgradesData = new();


    public void NewGame(string name)
    {
        Name = name;

        CurrentGameState = GameStateType.Init;
        PlayerAgentDatas = new();
        CurrencyData = new();
        GeneratorsData = new();
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
    [SerializeField] private int _amountToBuy = 1;

    public double TotalCurrency => _totalCurrency;
    public int AmountToBuy => _amountToBuy;

    public CurrencyData Load()
    {
        return this;
    }

    public void Save(double totalCurrency, int amountToBuy)
    {
        _totalCurrency = totalCurrency;
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
public class GeneratorsData
{
    [SerializeField] private List<GeneratorData> _generators = new();

    public GeneratorData Load(string guid)
    {
        return _generators.Find(x => x.Guid == guid);
    }

    public void Save(GeneratorData item)
    {
        if (!string.IsNullOrEmpty(item.Guid))
        {
            var oldItem = _generators.Find(x => x.Guid == item.Guid);
            _generators.ReplaceOrAdd(oldItem, item);
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
public class UpgradesData
{
    [SerializeField] private List<UpgradeData> _upgrades = new();

    public UpgradeData Load(string guid)
    {
        return _upgrades.Find(x => x.Guid == guid);
    }

    public void Save(UpgradeData item)
    {
        if (!string.IsNullOrEmpty(item.Guid))
        {
            var oldItem = _upgrades.Find(x => x.Guid == item.Guid);
            _upgrades.ReplaceOrAdd(oldItem, item);
        }
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
public class PlayerAgentDatas
{
    [SerializeField] private List<PlayerAgentData> _playerAgentDatas = new();

    public PlayerAgentData Load(string guid)
    {
        return _playerAgentDatas.Find(x => x.Guid == guid);
    }

    public void Save(PlayerAgentData item)
    {
        if (!string.IsNullOrEmpty(item.Guid))
        {
            var oldItem = _playerAgentDatas.Find(x => x.Guid == item.Guid);
            _playerAgentDatas.ReplaceOrAdd(oldItem, item);
        }
    }
}

[System.Serializable]
public class PlayerAgentData
{
    [SerializeField] private string _guid;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _totalExp;
    [SerializeField] private int _currentLevel;

    public string Guid => _guid;
    public int CurrentHealth => _currentHealth;
    public float TotalExp => _totalExp;
    public int CurrentLevel => _currentLevel;

    public PlayerAgentData(string guid, int currentHealth, float totalExp, int currentLevel)
    {
        _guid = guid;
        _currentHealth = currentHealth;
        _totalExp = totalExp;
        _currentLevel = currentLevel;
    }
}