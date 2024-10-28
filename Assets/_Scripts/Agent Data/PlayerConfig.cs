using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerConfig : MonoBehaviour, ISaveable
{
    [SerializeField] private PlayerAgentDataSO _config;
    private IAgent _agent;
    private MovementBehaviour _movement;

    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();
    internal MovementBehaviour Movement => _movement != null ? _movement : _movement = GetComponentInChildren<MovementBehaviour>();

    public void LoadData(GameData gameData)
    {
        var data = gameData.PlayerAgentDatas.Load(_config.Guid);
        if (data != null)
        {
            _config.SetCurrentHealth(data.CurrentHealth);
            _config.SetCurrentLevel(data.CurrentLevel);
            _config.SetTotalExp(data.TotalExp);
        }

        Agent.HealthSystem.Initialize(_config.CurrentHealth);
        Movement.SetMovementData(_config.MovementData);
    }

    public void SaveData(GameData gameData)
    {
        var agentData = new PlayerAgentData(
            _config.Guid,
            Agent.HealthSystem.GetCurrentHealth(),
            Agent.LevelSystem.GetTotalExp(),
            Agent.LevelSystem.GetCurrentLevel()
        );
        gameData.PlayerAgentDatas.Save(agentData);
    }
}
