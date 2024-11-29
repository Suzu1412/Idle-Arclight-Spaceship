using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/Persistence/GameDataSO")]
public class GameDataSO : ScriptableObject
{
	[SerializeField] private string _name;
	public string Name => _name;
	public GameData GameData;

	public List<GeneratorData> Generators => GameData.Generators;
	public List<PlayerAgentData> Players => GameData.Players;
	public List<UpgradeData> Upgrades => GameData.Upgrades;
	public List<UnlockSystemData> UnlockedSystems => GameData.UnlockedSystems;
	public CurrencyData CurrencyData => GameData.CurrencyData;
	public VolumeData VolumeData => GameData.VolumeData;
	public LocalizationData LocalizationData => GameData.LocalizationData;

	public List<GeneratorData> GetClearGeneratorDatas()
	{
		GameData.Generators.Clear();
		return GameData.Generators;
	}

	public void SaveGenerators(List<GeneratorData> generatorDatas)
	{
		GameData.Generators = generatorDatas;
	}

	public List<UpgradeData> GetClearUpgradeDatas()
	{
		GameData.Upgrades.Clear();
		return GameData.Upgrades;
	}

	public void SaveUpgrades(List<UpgradeData> upgradeDatas)
	{
		GameData.Upgrades = upgradeDatas;
	}

	public List<UnlockSystemData> GetClearUnlockedSystemsData()
	{
		GameData.UnlockedSystems.Clear();
		return GameData.UnlockedSystems;
	}

	public void SaveUnlockedSystems(List<UnlockSystemData> unlockedSystems)
	{
		GameData.UnlockedSystems = unlockedSystems;
	}

	public void SaveCurrency(double totalCurrency, double gameTotalCurrency, int amountToBuy)
	{
		GameData.CurrencyData = new(totalCurrency, gameTotalCurrency, amountToBuy);
	}

	public void SavePlayers(List<PlayerAgentData> players)
	{
		GameData.Players = players;
	}

	public void SaveVolume(float masterVolume, float musicVolume, float sfxVolume)
	{
		GameData.VolumeData = new(masterVolume, musicVolume, sfxVolume);
	}

	public void SaveLocalization(string locale)
	{
		GameData.LocalizationData = new(locale);
	}
}
