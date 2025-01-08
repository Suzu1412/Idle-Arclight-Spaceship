using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Destroyed State")]
public class MeteorDestroyedStateSO : StateSO<MeteorDestroyedContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;
    public GameObjectRuntimeSetSO ActiveMeteors => _activeMeteors;
}

[System.Serializable]
public class MeteorDestroyedContext : StateContext<MeteorDestroyedStateSO>
{
    public bool HasBeenExecuted = false;

    public override void OnExit()
    {
        base.OnExit();
        State.ActiveMeteors.Remove(_fsm.gameObject);
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.GetCurrentHealth() == 0 ? State.HighestUtility : 0;
    }
}