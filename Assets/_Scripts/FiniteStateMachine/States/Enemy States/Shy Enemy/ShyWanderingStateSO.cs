using UnityEngine;

[CreateAssetMenu(fileName = "ShyWanderingStateSO", menuName = "Scriptable Objects/ShyWanderingStateSO")]
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