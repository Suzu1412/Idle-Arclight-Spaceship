using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Spawn State")]
public class MeteorSpawnStateSO : StateSO<MeteorSpawnContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;

    protected override float _highestUtility => 999f;

    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        var context = GetContext(fsm, this);
        context.HasBeenExecuted = false;
        _activeMeteors.Add(fsm.gameObject);
        fsm.Agent.MoveBehaviour.StopMovement();
        context.HasBeenExecuted = true;
    }

    public override float EvaluateUtility(FiniteStateMachine fsm)
    {
        var context = GetContext(fsm, this);
        return !context.HasBeenExecuted ? _highestUtility : 0; // Never Return this
    }
}

[System.Serializable]
public class MeteorSpawnContext : StateContext
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