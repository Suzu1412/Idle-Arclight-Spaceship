using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/Persistence/GameDataSO")]
public class GameDataSO : ScriptableObject
{
	[SerializeField] private string _name;
	public string Name => _name;
	public GameData GameData;

	public async void LoadGenerators()
	{
		foreach (var generator in GameData.Generators)
		{
			var loadItemOperationHandle = Addressables.LoadAssetAsync<GeneratorSO>(generator.Guid);
			await loadItemOperationHandle.Task;
			if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				var generatorSO = loadItemOperationHandle.Result;
				generatorSO.SetAmount(generator.Amount);
				generatorSO.SetTotalProduction(generator.TotalProduction);
				generatorSO.IsUnlocked = generator.IsUnlocked;
			}
		}
	}

	public List<GeneratorData> GetClearGeneratorDatas()
	{
		GameData.Generators.Clear();
		return GameData.Generators;
	}

	public void SaveGenerators(List<GeneratorData> generatorDatas)
	{
		GameData.Generators = generatorDatas;
	}

	public async void LoadUpgrades()
	{
		foreach (var upgrade in GameData.Upgrades)
		{
			var loadItemOperationHandle = Addressables.LoadAssetAsync<BaseUpgradeSO>(upgrade.Guid);
			await loadItemOperationHandle.Task;
			if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				var upgradeSO = loadItemOperationHandle.Result;
				upgradeSO.IsRequirementMet = upgrade.IsRequirementMet;
				upgradeSO.ApplyUpgrade(upgrade.IsApplied);
			}
		}
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

	public CurrencyData LoadCurrency()
	{
		return GameData.CurrencyData;
	}

	public void SaveCurrency(double totalCurrency, double gameTotalCurrency, int amountToBuy)
	{
		GameData.CurrencyData = new(totalCurrency, gameTotalCurrency, amountToBuy);
	}
}
