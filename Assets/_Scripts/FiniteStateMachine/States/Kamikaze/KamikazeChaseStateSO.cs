using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeChaseStateSO", menuName = "Scriptable Objects/KamikazeChaseStateSO")]
public class KamikazeChaseStateSO : StateSO<KamikazeChaseContext>
{
}

[System.Serializable]
public class KamikazeChaseContext : StateContext<KamikazeChaseStateSO>
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