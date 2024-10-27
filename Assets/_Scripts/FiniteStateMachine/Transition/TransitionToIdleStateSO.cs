using UnityEngine;

[CreateAssetMenu(fileName = "TransitionToIdleState", menuName = "Scriptable Objects/Transition/TransitionToIdleStateSO")]
public class TransitionToIdleStateSO : BaseTransitionSO
{
    public override bool Condition(IAgent agent)
    {
        return agent.Input.Direction == Vector2.zero;
    }
}
