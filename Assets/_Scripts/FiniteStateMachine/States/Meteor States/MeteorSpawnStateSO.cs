using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Spawn State")]
public class MeteorSpawnStateSO : StateSO<MeteorSpawnContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;

    public GameObjectRuntimeSetSO ActiveMeteors => _activeMeteors;

    [SerializeField] private GameObjectRuntimeSetSO _enemyRTS;
    public GameObjectRuntimeSetSO EnemyRTS => _enemyRTS;
}

[System.Serializable]
public class MeteorSpawnContext : StateContext<MeteorSpawnStateSO>
{
    [SerializeField] private bool _hasBeenExecuted = false;

    public override void OnEnter()
    {
        base.OnEnter();
        State.ActiveMeteors.Add(_fsm.gameObject);
        State.EnemyRTS.Add(_fsm.gameObject);
        Agent.MoveBehaviour.StopMovement();
        _hasBeenExecuted = true;
    }

    public override float EvaluateUtility()
    {
        return !_hasBeenExecuted ? State.HighestUtility : 0f;
    }
}