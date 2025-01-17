using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeSpawnStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeSpawnStateSO")]
public class KamikazeSpawnStateSO : StateSO<KamikazeSpawnContext>
{
}



[System.Serializable]
public class KamikazeSpawnContext : StateContext<KamikazeSpawnStateSO>
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