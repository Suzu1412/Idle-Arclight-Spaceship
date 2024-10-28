using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerConfig : MonoBehaviour, ISaveable
{
    [SerializeField] private PlayerAgentDataSO _config;
    private List<PlayerAgentDataSO> _playerConfigs;
    private IAgent _agent;

    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();


    public void LoadData(GameData gameData)
    {
        var data = gameData.PlayerAgentDatas.Load(_config.Guid);
        if (data != null)
        {
            _config.SetCurrentHealth(data.CurrentHealth);
            _config.SetCurrentExp(data.CurrentExp);
        }

        Agent.HealthSystem.Initialize(_config.CurrentHealth);
    }

    public void SaveData(GameData gameData)
    {
        var agentData = new PlayerAgentData(
            _config.Guid,
            Agent.HealthSystem.GetCurrentHealth(),
            Agent.LevelSystem.GetCurrentExp()
        );
        gameData.PlayerAgentDatas.Save(agentData);
    }
}
