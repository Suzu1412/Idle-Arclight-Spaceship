using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateSO", menuName = "Scriptable Objects/State/Player StateSO")]
public class PlayerStateSO : BaseStateSO
{
    [SerializeReference][SubclassSelector] private PlayerState _state;

    public override BaseState State => _state;
}

[System.Serializable]
[HideInTypeMenu]
public class PlayerState : BaseState
{
}