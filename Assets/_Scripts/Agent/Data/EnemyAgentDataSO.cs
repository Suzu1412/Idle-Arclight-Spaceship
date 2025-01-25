using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAgentDataSO", menuName = "Scriptable Objects/Agent/EnemyAgentDataSO")]
public class EnemyAgentDataSO : AgentDataSO
{
    [SerializeField] protected DeathRewardSO _deathReward;
    [SerializeField] protected DropTableSO _dropTable;
    [SerializeField] protected Sprite[] _sprites;
    [SerializeField] protected RareVariant[] _rareVariant;
    [SerializeField] protected bool _isRare;


    public override void InitializeAgent(Agent agent, Vector3 position)
    {
        base.InitializeAgent(agent, position);
        agent.DropItem.SetDropTable(_dropTable);

        foreach (var rare in _rareVariant)
        {
            if (Random.value <= rare.SpawnChance)
            {
                _isRare = true;
                agent.transform.localScale = rare.Scale;
                agent.AgentRenderer.SpriteRenderer.sprite = rare.Sprite;
                break;
            }
        }

        if (!_isRare)
        {
            SetSprite(agent, 0);
        }
    }

    public void SetSprite(Agent agent, int sprite)
    {
        if (_isRare)
            return;

        agent.AgentRenderer.SpriteRenderer.sprite = _sprites[sprite];

    }

}

[System.Serializable]
public class RareVariant
{
    public Sprite Sprite { get; }
    public Vector3 Scale { get; }
    [Range(0.01f, 0.5f)] public float SpawnChance = 0.1f; // 10% chance by default
} 
