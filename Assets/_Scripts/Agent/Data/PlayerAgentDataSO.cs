using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAgentDataSO", menuName = "Scriptable Objects/Agent/PlayerAgentDataSO")]
public class PlayerAgentDataSO : AgentDataSO
{
    [SerializeField] private bool _isUnlocked;

    public bool IsUnlocked => _isUnlocked;
}
