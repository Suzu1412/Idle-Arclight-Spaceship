using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Destroyed State")]
public class MeteorDestroyedStateSO : StateSO<MeteorDestroyedContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;
    protected override float _highestUtility => 999f;

    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        _activeMeteors.Remove(fsm.gameObject);
    }

    public override float EvaluateUtility(FiniteStateMachine fsm)
    {
        return fsm.Agent.HealthSystem.GetCurrentHealth() == 0 ? _highestUtility : 0;
    }
}

[System.Serializable]
public class MeteorDestroyedContext : StateContext
{
    public bool HasBeenExecuted = false;

    public override void HandleMovement(Vector2 direction)
    {
        base.HandleMovement(direction);
    }

    public override void HandleAttack(bool isAttacking)
    {
        base.HandleAttack(isAttacking);
    }

    public override void Reset()
    {
        Timer = 0;
    }
}