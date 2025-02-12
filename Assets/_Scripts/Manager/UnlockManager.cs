using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour, ISaveable
{
	[SerializeField] private VoidGameEvent OnUnlockEvent;
	[SerializeField] private ListUnlockedSystemsSO _unlockedSystems;

	public void SaveData(GameDataSO gameData)
	{
		gameData.UnlockedSystems.Clear();
		List<UnlockSystemData> unlockedDatas = gameData.UnlockedSystems;
		foreach (var unlocked in _unlockedSystems.UnlockedSystems)
		{
			unlockedDatas.Add(new(unlocked.Guid, unlocked.IsUnlocked));
		}
		gameData.UnlockedSystems = unlockedDatas;
	}

	public void LoadData(GameDataSO gameData)
	{
		LoadUnlockedSystem(gameData.UnlockedSystems);
		OnUnlockEvent.RaiseEvent(this);
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}

	private void LoadUnlockedSystem(List<UnlockSystemData> unlockedDatas)
	{
		foreach (var unlockedSO in _unlockedSystems.UnlockedSystems)
		{
			unlockedSO.Initialize();
		}

		foreach (var unlocked in unlockedDatas)
		{
			var unlockedSO = _unlockedSystems.Find(unlocked.Guid);
			unlockedSO.IsUnlocked = unlocked.IsUnlocked;
		}
	}
}
