using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeChaseStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeChaseStateSO")]
public class KamikazeChaseStateSO : StateSO<KamikazeChaseContext>
{
}

[System.Serializable]
public class KamikazeChaseContext : StateContext<KamikazeChaseStateSO>
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Agent.TargetDetector.TargetTransform == null)
        {
            return;
        }
        var direction = _fsm.transform.position.GetDirectionTo(Agent.TargetDetector.TargetTransform.position);
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