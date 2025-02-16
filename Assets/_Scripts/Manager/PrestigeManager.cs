using System;
using UnityEngine;

public class PrestigeManager : MonoBehaviour, ISaveable
{
    [SerializeField] private DoubleVariableSO _prestigePoints;
    [SerializeField] private IntVariableSO _currentWaveTier;
    [SerializeField] private VoidGameEvent OnPrestigeProgressEvent; // Event for UI updates
    [SerializeField] private VoidGameEvent OnPrestigeEvent; // Event for Prestige Activated
    [SerializeField] private VoidGameEventBinding OnBossDefeatedEventBinding;
    [SerializeField] private int prestigeSkillPoints = 0;

    public double lifeTimePrestigePoints = 0; // TODO: CREATE UI AND EVENT TO DISPLAY THIS VALUE 
    public float totalPrestigePoints = 0f; 
    public int prestigeLevel = 0; // TODO: CREATE UI AND EVENT TO DISPLAY THIS VALUE 
    private event Action GrantPrestigePointsAction;

    public event Action<float, float> OnPrestigeProgressUpdated;

    private void Awake()
    {
        GrantPrestigePointsAction = GrantPrestigePoints;
    }

    private void OnEnable()
    {
        OnBossDefeatedEventBinding.Bind(GrantPrestigePointsAction, this);
    }

    private void OnDisable()
    {
        OnBossDefeatedEventBinding.Unbind(GrantPrestigePointsAction, this);
    }

    private void GrantPrestigePoints()
    {
        float points = 5f * Mathf.Pow(1.3f, _currentWaveTier.Value / 10f);
        totalPrestigePoints += points;
        lifeTimePrestigePoints += points;

        UpdatePrestigeProgress(); // Update UI
        Debug.Log($"Boss defeated at wave {_currentWaveTier.Value}! Gained {points} prestige points.");
    }

    public float GetPrestigeRequirement()
    {
        return 50f * Mathf.Pow(1.25f, prestigeLevel);
    }

    public bool CanPrestige()
    {
        return totalPrestigePoints >= GetPrestigeRequirement();
    }

    public void Prestige()
    {
        while (CanPrestige()) // Keep prestiging while possible
        {
            totalPrestigePoints -= GetPrestigeRequirement();
            prestigeLevel++;
            prestigeSkillPoints++;
            Debug.Log($"Prestiged! New level: {prestigeLevel}");
        }

        Debug.Log($"Prestiged! New level: {prestigeLevel}");
        ResetProgress();
    }

    private void UpdatePrestigeProgress()
    {
        double required = GetPrestigeRequirement();
        double progress = totalPrestigePoints / required;
        _prestigePoints.Initialize(progress, 0, required);
        OnPrestigeProgressEvent.RaiseEvent(this);
    }

    private void ResetProgress()
    {
        OnPrestigeEvent.RaiseEvent(this); // Reset game state
        UpdatePrestigeProgress(); // Update UI
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.PrestigeData = new(prestigeLevel, totalPrestigePoints, lifeTimePrestigePoints, prestigeSkillPoints);
    }

    public void LoadData(GameDataSO gameData)
    {
        prestigeLevel = gameData.PrestigeData.PrestigeLevel;
        totalPrestigePoints = gameData.PrestigeData.TotalPrestigePoints;
        lifeTimePrestigePoints = gameData.PrestigeData.LifeTimePrestigePoints;
        prestigeSkillPoints = gameData.PrestigeData.PrestigeSKillPoints;

        UpdatePrestigeProgress();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
