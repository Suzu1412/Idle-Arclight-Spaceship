using UnityEngine;

[System.Serializable]
public class PlayerIdleState : PlayerState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.MoveBehaviour.ApplyVelocity(Vector2.zero);
    }
}
