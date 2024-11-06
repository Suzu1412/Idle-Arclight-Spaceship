using UnityEngine;

[CreateAssetMenu(fileName = "Shy Enemy Wandering State SO", menuName = "Scriptable Objects/State/Enemy/Shy/Wandering State SO")]
public class ShyWanderingStateSO : BaseStateSO<ShyWanderingState>
{

}

public class ShyWanderingState : BaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        Agent.Input.CallOnMovementInput(Vector2.down);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }
}