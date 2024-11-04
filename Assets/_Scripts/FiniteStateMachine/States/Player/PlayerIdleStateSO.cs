using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIdleState", menuName = "Scriptable Objects/State/Player/PlayerIdleState")]
public class PlayerIdleStateSO : BaseStateSO<PlayerIdleState>
{

}

[System.Serializable]
public class PlayerIdleState : PlayerState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.MoveBehaviour.ApplyVelocity(Vector2.zero);
    }

}
