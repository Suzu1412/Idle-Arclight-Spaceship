using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class PlayerConfig : MonoBehaviour, ISaveable
{
    [SerializeField] private PlayerAgentDataSO _config;
    private PlayerAgentData _agentData;
    private IAgent _agent;

    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();


    public void LoadData(GameData gameData)
    {
        bool playerFound = false;

        if (!gameData.PlayerAgentDatas.IsNullOrEmpty())
        {
            for (int i = 0; i < gameData.PlayerAgentDatas.Count; i++)
            {
                Debug.Log(gameData.PlayerAgentDatas[i].Guid);
                if (gameData.PlayerAgentDatas[i].Guid == _config.Guid)
                {
                    playerFound = true;
                    _agentData = gameData.PlayerAgentDatas[i];
                    break;
                }
            }
        }
        
        if (playerFound)
        {
            _config.SetCurrentHealth(_agentData.CurrentHealth);
            _config.SetCurrentExp(_agentData.CurrentExp);
        }

        Agent.HealthSystem.Initialize(_config.CurrentHealth);
    }

    public void SaveData(GameData gameData)
    {
        Debug.Log("funciona siquiera?");

        _agentData = new()
        {
            Guid = _config.Guid,
            CurrentExp = Agent.LevelSystem.GetCurrentExp(),
            CurrentHealth = Agent.HealthSystem.GetCurrentHealth()
        };

        bool playerFound = false;
        if (!gameData.PlayerAgentDatas.IsNullOrEmpty())
        {
            for (int i = 0; i < gameData.PlayerAgentDatas.Count; i++)
            {
                if (gameData.PlayerAgentDatas[i].Guid == _config.Guid)
                {
                    playerFound = true;
                    gameData.PlayerAgentDatas[i] = _agentData;
                    break;
                }
            }

            if (!playerFound)
            {
                gameData.PlayerAgentDatas.Add(_agentData);
            }
        }
        else
        {
            gameData.PlayerAgentDatas = new()
            {
                _agentData
            };
        }
    }
}
