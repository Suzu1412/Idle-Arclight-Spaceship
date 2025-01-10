using UnityEngine;

[CreateAssetMenu(fileName = "Death State", menuName = "Scriptable Objects/FSM/Player/Death State")]
public class DeathStateSO : StateSO<DeathStateContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;

    public GameObjectRuntimeSetSO PlayerRTS => _playerRTS;
}
[System.Serializable]
public class DeathStateContext : StateContext<DeathStateSO>
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.Remove(_fsm.gameObject);
    }

    public override void OnExit()
    {
        base.OnExit();
        State.PlayerRTS.Remove(_fsm.gameObject);
    }

    protected override void HandleMovement(Vector2 direction)
    {
    }

    protected override void HandleAttack(bool isAttacking)
    {
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.IsDeath ? State.HighestUtility : 0f;
    }
}