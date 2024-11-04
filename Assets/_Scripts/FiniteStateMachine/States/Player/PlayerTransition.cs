using UnityEngine;

[System.Serializable]
public class PlayerTransition : BaseTransition
{
    [SerializeField] private PlayerStateSO _targetState;

    public override BaseStateSO TargetState => _targetState;
}
