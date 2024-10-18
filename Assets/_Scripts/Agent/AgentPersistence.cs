using UnityEngine;

public class AgentPersistence : MonoBehaviour, ISaveable
{
    [field: SerializeField] public SerializableGuid Id { get; set; }
    
    
    public void LoadData(GameData gameData)
    {

    }

    public void SaveData(GameData gameData)
    {
    }
}
