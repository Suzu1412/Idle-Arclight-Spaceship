using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDataSO", menuName = "Scriptable Objects/Agent/ObstacleDataSO")]
public class ObstacleDataSO : AgentDataSO
{
    [SerializeField] protected DropTableSO _dropTable;
    [SerializeField] protected Sprite _sprite;
    public int Cost = 10;

    public override void InitializeAgent(Agent agent, Vector3 position)
    {
        base.InitializeAgent(agent, position);
        agent.DropItem.SetDropTable(_dropTable);
        agent.AgentRenderer.SpriteRenderer.sprite = _sprite;
        //enemy.transform.GetChild(1).position = position; // set ui position
    }
}