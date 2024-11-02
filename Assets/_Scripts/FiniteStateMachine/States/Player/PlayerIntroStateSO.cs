using UnityEngine;

[CreateAssetMenu(fileName = "Player Intro State", menuName = "Scriptable Objects/State/PlayerIntroStateSO")]
public class PlayerIntroStateSO : BaseStateSO<PlayerIntroState>
{

}

public class PlayerIntroState : BaseState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.SetInvulnerability(true, 3f);        
        TransitionToIdleState();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}