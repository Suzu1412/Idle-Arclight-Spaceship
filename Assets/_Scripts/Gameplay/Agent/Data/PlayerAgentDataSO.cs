using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "PlayerAgentDataSO", menuName = "Scriptable Objects/Agent/PlayerAgentDataSO")]
public class PlayerAgentDataSO : AgentDataSO
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] private float _totalExp;

    [SerializeField] private bool _isUnlocked;// Tracks if this player is unlocked
    // Unlock-specific fields
    public string unlockCondition; // Description of how to unlock (optional)
    public int unlockCost; // Optional, for games with in-game currency

    public float TotalExp { get => _totalExp; internal set => _totalExp = value; }

    public bool IsUnlocked { get => _isUnlocked; internal set => _isUnlocked = value; }

    public override void InitializeAgent(Agent agent, Vector3 position)
    {
        base.InitializeAgent(agent, position);
        agent.AgentRenderer.SpriteRenderer.sprite = _sprite;
    }
}
