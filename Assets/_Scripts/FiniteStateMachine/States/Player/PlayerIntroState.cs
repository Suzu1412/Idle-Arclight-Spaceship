using UnityEngine;

[System.Serializable]
public class PlayerIntroState : PlayerState
{
    [SerializeField] private PlayerStateSO _playerIdle;

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.SetInvulnerability(true, 3f);
        _machine.Transition(_playerIdle);
    }
}
