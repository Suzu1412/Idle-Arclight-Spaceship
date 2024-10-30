using UnityEngine;

public class AgentData : MonoBehaviour, ISaveable
{
    [SerializeField] private AgentDataSO _data;
    private IAgent _agent;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    public void LoadData(GameData gameData)
    {

    }

    public void SaveData(GameData gameData)
    {
    }
}
