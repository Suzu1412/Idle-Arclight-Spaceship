using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeChaseAcionSO", menuName = "Scriptable Objects/KamikazeChaseAcionSO")]
public class KamikazeChaseAcionSO : ActionSO
{
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;
    [SerializeField] private SoundDataSO _impactSound;
    [SerializeField] private float _range = 1f;
    [SerializeField] private Color _color = Color.green;

    public override void DrawGizmos(FiniteStateMachine fsm)
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(fsm.transform.position, _range);
    }

    public override void Execute(FiniteStateMachine fsm)
    {
        bool detected = false;
        float closestTarget = float.MaxValue;
        float currentTarget = 0f;
        Vector2 targetPosition = Vector2.zero;

        foreach (var player in _playerRTS.Items)
        {
            if (fsm.transform.position.IsWithinRange(player.transform.position, _range))
            {
                currentTarget = fsm.transform.position.GetSquaredDistanceTo(player.transform.position);
                detected = true;

                if (currentTarget < closestTarget)
                {
                    closestTarget = currentTarget;
                    targetPosition = player.transform.position;
                }
            }
        }

        if (detected)
        {
            Vector2 direction = fsm.transform.position.GetDirectionTo(targetPosition);
            fsm.Agent.Input.CallOnMovementInput(direction);
        }
    }
}
