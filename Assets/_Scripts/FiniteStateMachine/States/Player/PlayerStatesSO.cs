using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player States", menuName = "Scriptable Objects/State/Player/PlayerStatesSO")]
public class PlayerStatesSO : BaseStatesSO
{

    [SerializeReference]
    [SubclassSelector]
    private List<BaseState> playerState;

    public override List<IState> States => (List<IState>)(playerState as IState);

}
