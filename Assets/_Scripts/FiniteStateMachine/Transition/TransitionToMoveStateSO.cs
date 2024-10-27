using UnityEngine;

[CreateAssetMenu(fileName = "TransitionToMoveStateSO", menuName = "Scriptable Objects/Transition/TransitionToMoveStateSO")]
public class TransitionToMoveStateSO : BaseTransitionSO
{
    public override bool Condition(IAgent agent)
    {
        return agent.Input.Direction != Vector2.zero;
    }
}
