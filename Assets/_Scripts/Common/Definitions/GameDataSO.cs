using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/Persistence/GameDataSO")]
public class GameDataSO : ScriptableObject, ISaveableData
{
	[SerializeField] private string _name;
	public string Name => _name;
	[Header("Generators")]
	public List<GeneratorData> Generators;
	public List<PlayerAgentData> Players;
	public List<UpgradeData> Upgrades;
	public List<UnlockSystemData> UnlockedSystems;
	public CurrencyData CurrencyData;
	public PrestigeData PrestigeData;
	private int _amountToBuy;
	public int AmountToBuy => _amountToBuy;

	public void Initialize()
	{
		CurrencyData = new();
		Generators = new();
		Upgrades = new();
		Players = new();
		CurrencyData = new();
		UnlockedSystems = new();
		PrestigeData = new();
		SetAmountToBuy(1);
	}

	public void SetAmountToBuy(int amount)
	{
		if (amount < 0)
		{
			_amountToBuy = -1;
		}
		else if (amount > 100)
		{
			_amountToBuy = -1;
		}
		else
		{
			_amountToBuy = amount;
		}
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
	[SerializeField] private BigNumber _lifetimeCurrency;
	[SerializeField] private BigNumber _totalCurrency;

	[SerializeField] private BigNumber _highestProduction;

	[SerializeField] private double _gameTotalCurrency;
	[SerializeField] private int _amountToBuy = 1;
	[SerializeField] private long _lastActiveDateTime;

	public BigNumber LifetimeCurrency => _lifetimeCurrency;
	public BigNumber TotalCurrency => _totalCurrency;
	public BigNumber HighestProduction => _highestProduction;
	public int AmountToBuy => _amountToBuy;
	public long LastActiveDateTime => _lastActiveDateTime;

	public CurrencyData()
	{
		_lifetimeCurrency = BigNumber.Zero;
		_totalCurrency = BigNumber.Zero;
		_highestProduction = BigNumber.Zero;
		_lastActiveDateTime = DateTime.UtcNow.Ticks;
	}

	public CurrencyData(BigNumber lifetimeCurrency, BigNumber totalCurrency, BigNumber highestProduction)
	{
		_lifetimeCurrency = lifetimeCurrency;
		_totalCurrency = totalCurrency;
		_highestProduction = highestProduction;
		_lastActiveDateTime = DateTime.UtcNow.Ticks;
	}


}


[System.Serializable]
public class GeneratorData
{
	[SerializeField] private string _guid;
	[SerializeField] private int _amount;
	[SerializeField] private BigNumber _totalProduction;
	[SerializeField] private bool _isVisibleInStore = false;
	[SerializeField] private bool _shouldNotify = true;

	public string Guid => _guid;
	public int Amount => _amount;
	public BigNumber TotalProduction => _totalProduction;
	public bool IsVisibleInStore => _isVisibleInStore;

	public GeneratorData(string guid, int amount, BigNumber totalProduction, bool isVisibleInStore)
	{
		_guid = guid;
		_amount = amount;
		_totalProduction = totalProduction;
		_isVisibleInStore = isVisibleInStore;
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
public class PrestigeData
{
	[SerializeField] private int _prestigeLevel = 0;
	[SerializeField] private float _totalPrestigePoints = 0;
	[SerializeField] private double _lifeTimePrestigePoints = 0;
	[SerializeField] private int _prestigeSkillPoints = 0;

	public int PrestigeLevel => _prestigeLevel;
	public float TotalPrestigePoints => _totalPrestigePoints;
	public double LifeTimePrestigePoints => _lifeTimePrestigePoints;
	public int PrestigeSKillPoints => _prestigeSkillPoints;

	public PrestigeData()
	{
		_prestigeLevel = 0;
		_totalPrestigePoints = 0f;
		_lifeTimePrestigePoints = 0f;
		_prestigeSkillPoints = 0;
	}

	public PrestigeData(int prestigeLevel, float totalPrestigePoints, double lifeTimePrestigePoints, int prestigeSkillPoints)
	{
		_prestigeLevel = prestigeLevel;
		_totalPrestigePoints = totalPrestigePoints;
		_lifeTimePrestigePoints = lifeTimePrestigePoints;
		_prestigeSkillPoints = prestigeSkillPoints;
	}

}