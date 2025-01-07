using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Falling State")]
public class MeteorFallingStateSO : StateSO<MeteorFallingContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;
    protected override float _highestUtility => 10f;

    [SerializeField] private float fallingSpeed = 5f;
    [SerializeField] private float trajectoryVariation = 2f;
    [SerializeField] private float boundaryX = 2.3f; // Horizontal screen limits
    [SerializeField] private float elasticity = 0.8f;
    [SerializeField] private float angularBounceFactor = 10f; // Controls rotational speed after bounce

    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        var direction = new Vector2(Random.Range(-trajectoryVariation, trajectoryVariation), -fallingSpeed);
        fsm.Agent.Input.CallOnMovementInput(direction);
        fsm.Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }

    public override void UpdateState(FiniteStateMachine fsm)
    {
        base.UpdateState(fsm);
        var context = GetContext(fsm, this);
        context.UpdateBounceTimerDeltaTime(Time.deltaTime);
        // Optional: Gradually adjust trajectory while falling
        Vector2 randomForce = new Vector2(
            Random.Range(-trajectoryVariation * 0.1f, trajectoryVariation * 0.1f),
            0 // Keep the force horizontal to add wobble
        );

        fsm.Agent.MoveBehaviour.ApplyForce(randomForce, ForceMode2D.Force);
        HandleBoundaryBounce(fsm, context);
    }

    public override void FixedUpdateState(FiniteStateMachine fsm)
    {
        base.FixedUpdateState(fsm);
        var context = GetContext(fsm, this);
        fsm.Agent.MoveBehaviour.Move();
        //HandleMeteorCollisions();
    }

    public override void ExitState(FiniteStateMachine fsm)
    {
        base.ExitState(fsm);
        _activeMeteors.Remove(fsm.gameObject);
    }

    public override float EvaluateUtility(FiniteStateMachine fsm)
    {
        return _highestUtility;
    }

    private void HandleBoundaryBounce(FiniteStateMachine fsm, MeteorFallingContext context)
    {
        if (context.BounceCooldown > 0f)
        {
            return;
        }
        Vector2 position = fsm.transform.position;

        if (position.x <= -boundaryX)
        {
            Debug.Log(position.x);
        }

        // Check horizontal boundaries
        if (position.x <= -boundaryX && fsm.Agent.MoveBehaviour.RB.linearVelocityX < 0)
        {
            Vector2 direction = new(-fsm.Agent.Input.Direction.x * elasticity, fsm.Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(-boundaryX, position.y); // Clamp position
            fsm.Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-angularBounceFactor, angularBounceFactor); // Add spin
            fsm.Agent.Input.CallOnMovementInput(direction);
            fsm.Agent.AgentRenderer.RotateSpriteToDirection(direction);

        }
        else if (position.x >= boundaryX && fsm.Agent.MoveBehaviour.RB.linearVelocityX > 0)
        {
            Vector2 direction = new(-fsm.Agent.Input.Direction.x * elasticity, fsm.Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(boundaryX, position.y); // Clamp position
            fsm.Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-angularBounceFactor, angularBounceFactor); // Add spin
            fsm.Agent.Input.CallOnMovementInput(direction);
            fsm.Agent.AgentRenderer.RotateSpriteToDirection(direction);
        }

        context.BounceCooldown = 0.2f;
    }
    
    /*
    private void HandleMeteorCollisions()
    {
        foreach (var otherMeteor in _activeMeteors.Items)
        {
            if (otherMeteor == this) continue;

            Vector2 positionA = transform.position;
            Vector2 positionB = otherMeteor.transform.position;

            // Calculate the distance between this meteor and the other
            float distance = Vector2.Distance(positionA, positionB);
            float radiusSum = 0.5f; // Assume meteors have a radius of 0.5 units

            if (distance < radiusSum)
            {
                ResolveCollision(otherMeteor);
            }
        }
    }

    private void ResolveCollision(Meteor otherMeteor)
    {
        Vector2 positionA = transform.position;
        Vector2 positionB = otherMeteor.transform.position;

        Vector2 velocityA = rb.velocity;
        Vector2 velocityB = otherMeteor.rb.velocity;

        // Normal vector between the meteors
        Vector2 collisionNormal = (positionB - positionA).normalized;

        // Project velocities onto the normal
        float velocityAlongNormalA = Vector2.Dot(velocityA, collisionNormal);
        float velocityAlongNormalB = Vector2.Dot(velocityB, collisionNormal);

        // Exchange velocities along the normal
        float temp = velocityAlongNormalA;
        velocityAlongNormalA = velocityAlongNormalB;
        velocityAlongNormalB = temp;

        // Update velocities
        rb.velocity = velocityA - velocityAlongNormalA * collisionNormal + velocityAlongNormalB * collisionNormal;
        otherMeteor.rb.velocity = velocityB - velocityAlongNormalB * collisionNormal + velocityAlongNormalA * collisionNormal;
    }
    */
}


[System.Serializable]
public class MeteorFallingContext : StateContext
{
    public bool HasBeenExecuted = false;
    public float BounceCooldown;

    public override void HandleMovement(Vector2 direction)
    {
        base.HandleMovement(direction);
        Agent.MoveBehaviour.ReadInputDirection(direction);
    }

    public override void HandleAttack(bool isAttacking)
    {
        base.HandleAttack(isAttacking);
    }

    public void UpdateBounceTimerDeltaTime(float seconds)
    {
        BounceCooldown -= seconds;

        if (BounceCooldown < 0f)
        {
            BounceCooldown = 0f;
        }
    }

    public override void Reset()
    {
        Timer = 0;
    }
}