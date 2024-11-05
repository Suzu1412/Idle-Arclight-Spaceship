using UnityEngine;

public class MeteorStateSO : BaseStateSO
{
    [SerializeReference][SubclassSelector] private MeteorState _state;

    public override BaseState State => _state;
}

[System.Serializable]
[HideInTypeMenu]
public class MeteorState : BaseState
{

}
