using UnityEngine;

[CreateAssetMenu(fileName = "Meteor StateSO", menuName = "Scriptable Objects/State/Enemy/Meteor StateSO")]
public class MeteorStateSO : BaseStateSO
{
    [SerializeReference] [SubclassSelector] private MeteorState _state;

    public override BaseState State => _state;
}

[System.Serializable]
[HideInTypeMenu]
public class MeteorState : BaseState
{

}
