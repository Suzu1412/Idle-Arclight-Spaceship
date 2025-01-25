using UnityEngine;

public class MeteorCollissionBounceActionSO : ActionSO<MeteorContext>
{
    public override void DrawGizmos(MeteorContext context)
    {
    }

    public override void Execute(MeteorContext context)
    {

    }

    //private void HandleMeteorCollisions(FiniteStateMachine fsm)
    //{

    //    foreach (var otherMeteor in _activeMeteors.Items)
    //    {
    //        if (otherMeteor == this) continue;

    //        Vector2 positionA = fsm.transform.position;
    //        Vector2 positionB = otherMeteor.transform.position;

    //        // Calculate the distance between this meteor and the other
    //        float distance = Vector2.Distance(positionA, positionB);
    //        float radiusSum = 0.5f; // Assume meteors have a radius of 0.5 units

    //        if (distance < radiusSum)
    //        {
    //            ResolveCollision(fsm, otherMeteor);
    //        }
    //    }
    //}

    //private void ResolveCollision(FiniteStateMachine fsm, GameObject otherMeteor)
    //{
    //    Vector2 positionA = fsm.transform.position;
    //    Vector2 positionB = otherMeteor.transform.position;

    //    Vector2 velocityA = rb.velocity;
    //    Vector2 velocityB = otherMeteor.rb.velocity;

    //    // Normal vector between the meteors
    //    Vector2 collisionNormal = (positionB - positionA).normalized;

    //    // Project velocities onto the normal
    //    float velocityAlongNormalA = Vector2.Dot(velocityA, collisionNormal);
    //    float velocityAlongNormalB = Vector2.Dot(velocityB, collisionNormal);

    //    // Exchange velocities along the normal
    //    float temp = velocityAlongNormalA;
    //    velocityAlongNormalA = velocityAlongNormalB;
    //    velocityAlongNormalB = temp;

    //    // Update velocities
    //    rb.velocity = velocityA - velocityAlongNormalA * collisionNormal + velocityAlongNormalB * collisionNormal;
    //    otherMeteor.rb.velocity = velocityB - velocityAlongNormalB * collisionNormal + velocityAlongNormalA * collisionNormal;
    //}
}
