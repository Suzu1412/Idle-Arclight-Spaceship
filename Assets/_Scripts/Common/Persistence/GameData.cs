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
    public PlayerAgentDatas PlayerAgentDatas;

    [Header("Currency")]
    public CurrencyData CurrencyData;

    [Header("Generators")]
    public GeneratorsData GeneratorsData;

    public void NewGame(string name)
    {
        Name = name;

        CurrentGameState = GameStateType.Init;
        PlayerAgentDatas = new();
        CurrencyData = new CurrencyData();
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
    [SerializeField] private int _amountToBuy;

    public double TotalCurrency => _totalCurrency;
    public int AmountToBuy => _amountToBuy;

    public CurrencyData Load()
    {
        return this;
    }

    public void Save(double totalCurrency, int amountToBuy)
    {
        _totalCurrency = totalCurrency;
        _amountToBuy = amountToBuy;
    }
}



[System.Serializable]
public class GeneratorsData
{
    [SerializeField] private List<GeneratorData> _generators = new();

    public void New()
    {
        _generators = new();
    }

    public GeneratorData Load(string guid)
    {
        return _generators.Find(x => x.Guid == guid);
    }

    public void Save(GeneratorData generator)
    {
        if (!string.IsNullOrEmpty(generator.Guid))
        {
            var oldItem = _generators.Find(x => x.Guid == generator.Guid);
            _generators.ReplaceOrAdd(oldItem, generator);
        }
    }
}

[System.Serializable]
public class GeneratorData
{
    [SerializeField] private string _guid;
    [SerializeField] private int _amount;

    public string Guid => _guid;
    public int Amount => _amount;

    public GeneratorData(string guid, int amount)
    {
        _guid = guid;
        _amount = amount;
    }
}

[System.Serializable]
public class PlayerAgentDatas
{
    [SerializeField] private List<PlayerAgentData> _playerAgentDatas = new();

    public void New()
    {
        _playerAgentDatas = new();
    }

    public PlayerAgentData Load(string guid)
    {
        return _playerAgentDatas.Find(x => x.Guid == guid);
    }

    public void Save(PlayerAgentData agentData)
    {
        if (!string.IsNullOrEmpty(agentData.Guid))
        {
            var oldItem = _playerAgentDatas.Find(x => x.Guid == agentData.Guid);
            _playerAgentDatas.ReplaceOrAdd(oldItem, agentData);
        }
    }
}

[System.Serializable]
public class PlayerAgentData
{
    [SerializeField] private string _guid;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _currentExp;

    public string Guid => _guid;
    public int CurrentHealth => _currentHealth;
    public float CurrentExp => _currentExp;

    public PlayerAgentData(string guid, int currentHealth, float currentExp)
    {
        _guid = guid;
        _currentHealth = currentHealth;
        _currentExp = currentExp;
    }
}