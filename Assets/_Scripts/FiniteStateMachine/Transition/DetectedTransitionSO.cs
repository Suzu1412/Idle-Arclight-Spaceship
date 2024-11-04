using UnityEngine;

[CreateAssetMenu(fileName = "On Detected Transition", menuName = "Scriptable Objects/Transition/Enemy/DetectedTransitionSo")]
public class DetectedTransitionSO : BaseTransitionSO
{
    public override bool Condition(IAgent agent)
    {
        return agent.PlayerDetector.IsDetected;
    }
}
