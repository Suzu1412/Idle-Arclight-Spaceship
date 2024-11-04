using UnityEngine;


public class ShyChaseStateSO : BaseStateSO<ShyChaseState>
{

}

public class ShyChaseState : BaseState
{
    private Transform _player;

    public override void OnEnter()
    {
        base.OnEnter();
        _player = Agent.PlayerDetector.PlayerDetected;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }


}