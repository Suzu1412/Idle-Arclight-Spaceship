using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Falling State")]
public class MeteorFallingStateSO : StateSO<MeteorContext>
{
    [SerializeField] private float _fallingSpeed = 5f;
    [SerializeField] private float _trajectoryVariation = 2f;
    [SerializeField] private float _boundaryX = 2.3f; // Horizontal screen limits
    [SerializeField] private float _elasticity = 0.8f;
    [SerializeField] private float _angularBounceFactor = 10f; // Controls rotational speed after bounce
    [SerializeField] private float _bounceCooldown = 0.2f;

    public float FallingSpeed => _fallingSpeed;
    public float TrajectoryVariation => _trajectoryVariation;
    public float BoundaryX => _boundaryX;
    public float Elasticity => _elasticity;
    public float AngularBounceFactor => _angularBounceFactor;

    public float BounceCooldown => _bounceCooldown;

    public override float EvaluateUtility(MeteorContext context)
    {
        return context.Agent.HealthSystem.GetCurrentHealth() > 0f ? HighestUtility : 0f;
    }

    public override void OnEnter(MeteorContext context)
    {
        var direction = new Vector2(Random.Range(-TrajectoryVariation, TrajectoryVariation), -FallingSpeed);
        context.Agent.Input.CallOnMovementInput(direction);
        context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }

    public override void OnExit(MeteorContext context)
    {
    }

    public override void OnFixedUpdate(MeteorContext context)
    {
        context.Agent.MoveBehaviour.Move();
        //HandleMeteorCollisions();
    }

    public override void OnUpdate(MeteorContext context)
    {
        context.BounceTick();

        // Optional: Gradually adjust trajectory while falling
        Vector2 randomForce = new Vector2(
            Random.Range(-TrajectoryVariation * 0.1f, TrajectoryVariation * 0.1f),
            0 // Keep the force horizontal to add wobble
        );

        context.Agent.MoveBehaviour.ApplyForce(randomForce, ForceMode2D.Force);
        HandleBoundaryBounce(context);
    }

    private void HandleBoundaryBounce(MeteorContext context)
    {
        if (context.BounceCooldown > 0f)
        {
            return;
        }
        Vector2 position = context.FSM.transform.position;

        // Check horizontal boundaries
        if (position.x <= -BoundaryX && context.Agent.MoveBehaviour.RB.linearVelocityX < 0)
        {
            Vector2 direction = new(-context.Agent.Input.Direction.x * Elasticity, context.Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(-boundaryX, position.y); // Clamp position
            context.Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-AngularBounceFactor, AngularBounceFactor); // Add spin
            context.Agent.Input.CallOnMovementInput(direction);
            context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
            context.BounceCooldown = BounceCooldown;
        }
        else if (position.x >= BoundaryX && context.Agent.MoveBehaviour.RB.linearVelocityX > 0)
        {
            Vector2 direction = new(-context.Agent.Input.Direction.x * Elasticity, context.Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(boundaryX, position.y); // Clamp position
            context.Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-AngularBounceFactor, AngularBounceFactor); // Add spin
            context.Agent.Input.CallOnMovementInput(direction);
            context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
            context.BounceCooldown = BounceCooldown;
        }

    }
}