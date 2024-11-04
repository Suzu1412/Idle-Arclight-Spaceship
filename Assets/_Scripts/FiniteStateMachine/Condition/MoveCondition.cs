using UnityEngine;

[System.Serializable]
public class MoveCondition : ICondition
{
    [SerializeField] private bool _expectedResult;

    public bool Evaluate(IAgent agent)
    {
        return agent.Input.Direction != Vector2.zero == _expectedResult;
    }
}
