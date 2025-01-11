using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Spawn State")]
public class MeteorSpawnStateSO : StateSO<MeteorSpawnContext>
{
}

[System.Serializable]
public class MeteorSpawnContext : StateContext<MeteorSpawnStateSO>
{
    [SerializeField] private bool _hasBeenExecuted = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.MoveBehaviour.StopMovement();
        _hasBeenExecuted = true;
    }

    public override float EvaluateUtility()
    {
        return !_hasBeenExecuted ? State.HighestUtility : 0f;
    }
}