using UnityEngine;

[System.Serializable]
public class HealthCondition : ICondition
{
    [SerializeField] [Range(0f, 1f)] private float _percent = 0.5f;
    [SerializeField] private bool _isHigherThan;

    public bool Evaluate(IAgent agent)
    {
        return agent.HealthSystem.GetHealthPercent() >= _percent == _isHigherThan;
    }
}
