using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeWanderStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeWanderStateSO")]
public class KamikazeWanderStateSO : StateSO<KamikazeWanderContext>
{
    
}

[System.Serializable]
public class KamikazeWanderContext : StateContext<KamikazeWanderStateSO>
{
    public override void OnEnter()
    {
        base.OnEnter();
        var direction = Vector2.down;
        Agent.Input.CallOnMovementInput(direction);
        Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }

    public override float EvaluateUtility()
    {
        return Agent.TargetDetector.IsDetected ? State.HighestUtility : 0f;
    }
}