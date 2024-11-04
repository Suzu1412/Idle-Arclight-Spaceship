using UnityEngine;

[System.Serializable]
public class DetectedCondition : ICondition
{
    [SerializeField] private bool _expectedResult;

    public bool Evaluate(IAgent agent)
    {
        return agent.PlayerDetector.IsDetected == _expectedResult;
    }
}
