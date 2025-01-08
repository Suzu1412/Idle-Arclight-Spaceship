using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Attack State")]
public class AttackStateSO : StateSO<AttackStateContext>
{

}

[System.Serializable]
public class AttackStateContext : StateContext<AttackStateSO>
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        Agent.Input.CallOnAttack(true);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
        Agent.MoveBehaviour.BoundMovement();
    }

    public override float EvaluateUtility()
    {
        return Agent.PlayerDetector.IsDetected ? State.HighestUtility : 0f;

    }
}