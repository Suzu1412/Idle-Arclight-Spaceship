using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIdleState", menuName = "Scriptable Objects/State/PlayerIdleState")]
public class PlayerIdleStateSO : BaseStateSO<PlayerIdleState>
{

}

public class PlayerIdleState : BaseState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.MoveBehaviour.ApplyVelocity(Vector2.zero);
    }
}
