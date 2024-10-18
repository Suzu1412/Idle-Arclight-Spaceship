using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IAgent _agent;

    internal IAgent Agent => _agent ??= GetComponentInChildren<IAgent>();

    private void OnEnable()
    {
        Agent.Input.OnMovement += HandleMovement;
    }

    private void OnDisable()
    {
        Agent.Input.OnMovement -= HandleMovement;
    }

    private void HandleMovement(Vector2 direction)
    {
        Agent.MoveBehaviour.ReadInputDirection(direction);
    }

}
