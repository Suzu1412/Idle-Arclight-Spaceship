using UnityEngine;

[CreateAssetMenu(fileName = "Player Intro State SO", menuName = "Scriptable Objects/State/Player/Intro State SO")]
public class PlayerIntroStateSO : BaseStateSO<PlayerIntroState>
{
    [SerializeField] private BaseStateSO _playerIdle;

    public BaseStateSO PlayerIdle => _playerIdle;
}

public class PlayerIntroState : BaseState
{
    private BaseStateSO _playerIdle;

    public override void OnEnter()
    {
        base.OnEnter();
        _playerIdle = (_origin as PlayerIntroStateSO).PlayerIdle;
        Agent.HealthSystem.SetInvulnerability(true, 3f);
        if (_playerIdle == null)
        {
            Debug.LogError($"Player missing: {_playerIdle}");
        }
        _machine.Transition(_playerIdle);
    }
}
