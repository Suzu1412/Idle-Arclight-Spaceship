using Unity.Cinemachine;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IKillable>(out var killable))
        {
            killable.Death(gameObject, DeathCauseType.DeadZone);
        }

        if (collision.TryGetComponent<IRemovable>(out var other))
        {
            other.Remove(gameObject);
            return;
        }
    }
}
