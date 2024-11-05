using UnityEngine;

[CreateAssetMenu(fileName = "Shy Enemy State SO", menuName = "Scriptable Objects/State/Enemy/Shy Enemy State SO")]
public class ShyEnemyStateSO : BaseStateSO
{
    [SerializeReference] [SubclassSelector] private ShyEnemyState _state;

    public override BaseState State => _state;
}

[System.Serializable]
[HideInTypeMenu]
public class ShyEnemyState : BaseState
{

}

[System.Serializable]
public class ShyTransition : BaseTransition
{
    [SerializeField] private ShyEnemyStateSO _targetState;

    public override BaseStateSO TargetState => _targetState;
}
