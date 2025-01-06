using UnityEngine;

[System.Serializable]
public abstract class StateContext
{
    public IAgent Agent;
    public float Timer;

    public void Initialize(IAgent agent)
    {
        Agent = agent;
    }

    public void SetTimer(float timer)
    {
        Timer = timer;
    }

    public virtual void Reset()
    {
        Timer = 0f;
    }

    public void UpdateTimerDeltaTime()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0f)
        {
            Timer = 0f;
        }
    }

    public void EnableEvents()
    {
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.Attack += HandleAttack;
    }

    public void DisableEvents()
    {
        Agent.Input.OnMovement -= HandleMovement;
        Agent.Input.Attack -= HandleAttack;
    }

    public virtual void HandleMovement(Vector2 direction)
    {
        Agent.MoveBehaviour.ReadInputDirection(direction);
    }

    public virtual void HandleAttack(bool isAttacking)
    {
        Agent.AttackSystem.Attack(isAttacking);
    }
}
