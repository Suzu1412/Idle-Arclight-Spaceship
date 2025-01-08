using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Falling State")]
public class MeteorFallingStateSO : StateSO<MeteorFallingContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _activeMeteors;
    [SerializeField] private float _fallingSpeed = 5f;
    [SerializeField] private float _trajectoryVariation = 2f;
    [SerializeField] private float _boundaryX = 2.3f; // Horizontal screen limits
    [SerializeField] private float _elasticity = 0.8f;
    [SerializeField] private float _angularBounceFactor = 10f; // Controls rotational speed after bounce
    [SerializeField] private float _bounceCooldown = 0.2f;

    public GameObjectRuntimeSetSO ActiveMeteors => _activeMeteors;
    public float FallingSpeed => _fallingSpeed;
    public float TrajectoryVariation => _trajectoryVariation;
    public float BoundaryX => _boundaryX;
    public float Elasticity => _elasticity;
    public float AngularBounceFactor => _angularBounceFactor;

    public float BounceCooldown => _bounceCooldown;

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
public class MeteorFallingContext : StateContext<MeteorFallingStateSO>
{
    private float _bounceCooldown;

    public override void OnEnter()
    {
        base.OnEnter();
        var direction = new Vector2(Random.Range(-State.TrajectoryVariation, State.TrajectoryVariation), -State.FallingSpeed);
        Agent.Input.CallOnMovementInput(direction);
        Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        UpdateBounceCooldown(Time.deltaTime);

        // Optional: Gradually adjust trajectory while falling
        Vector2 randomForce = new Vector2(
            Random.Range(-State.TrajectoryVariation * 0.1f, State.TrajectoryVariation * 0.1f),
            0 // Keep the force horizontal to add wobble
        );

        Agent.MoveBehaviour.ApplyForce(randomForce, ForceMode2D.Force);
        HandleBoundaryBounce();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
        //HandleMeteorCollisions();
    }

    public override void OnExit()
    {
        base.OnExit();
        State.ActiveMeteors.Remove(_fsm.gameObject);
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.GetCurrentHealth() > 0f ? State.HighestUtility : 0f;
    }

    private void HandleBoundaryBounce()
    {
        if (_bounceCooldown > 0f)
        {
            return;
        }
        Vector2 position = _fsm.transform.position;

        // Check horizontal boundaries
        if (position.x <= -State.BoundaryX && Agent.MoveBehaviour.RB.linearVelocityX < 0)
        {
            Vector2 direction = new(-Agent.Input.Direction.x * State.Elasticity, Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(-boundaryX, position.y); // Clamp position
            Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-State.AngularBounceFactor, State.AngularBounceFactor); // Add spin
            Agent.Input.CallOnMovementInput(direction);
            Agent.AgentRenderer.RotateSpriteToDirection(direction);

        }
        else if (position.x >= State.BoundaryX && Agent.MoveBehaviour.RB.linearVelocityX > 0)
        {
            Vector2 direction = new(-Agent.Input.Direction.x * State.Elasticity, Agent.Input.Direction.y); // Reverse horizontal velocity
            //agent.MoveBehaviour.RB.position = new Vector2(boundaryX, position.y); // Clamp position
            Agent.MoveBehaviour.RB.angularVelocity = Random.Range(-State.AngularBounceFactor, State.AngularBounceFactor); // Add spin
            Agent.Input.CallOnMovementInput(direction);
            Agent.AgentRenderer.RotateSpriteToDirection(direction);
        }

        _bounceCooldown = State.BounceCooldown;
    }

    private void UpdateBounceCooldown(float seconds)
    {
        _bounceCooldown -= seconds;

        if (_bounceCooldown < 0f)
        {
            _bounceCooldown = 0f;
        }
    }
}