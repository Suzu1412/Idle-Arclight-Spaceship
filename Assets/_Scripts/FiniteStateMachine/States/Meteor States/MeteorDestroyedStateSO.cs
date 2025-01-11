using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Destroyed State")]
public class MeteorDestroyedStateSO : StateSO<MeteorDestroyedContext>
{
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
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.IsDeath ? State.HighestUtility : 0;
    }
}