using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Destroyed State")]
public class MeteorDestroyedStateSO : StateSO<MeteorDestroyedContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;
    public GameObjectRuntimeSetSO ActiveMeteors => _activeMeteors;

    [SerializeField] private GameObjectRuntimeSetSO _enemyRTS;
    public GameObjectRuntimeSetSO EnemyRTS => _enemyRTS;
}

[System.Serializable]
public class MeteorDestroyedContext : StateContext<MeteorDestroyedStateSO>
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.Remove(_fsm.gameObject);
    }

    public override void OnExit()
    {
        base.OnExit();
        State.ActiveMeteors.Remove(_fsm.gameObject);
        State.EnemyRTS.Remove(_fsm.gameObject);
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.IsDeath ? State.HighestUtility : 0;
    }
}