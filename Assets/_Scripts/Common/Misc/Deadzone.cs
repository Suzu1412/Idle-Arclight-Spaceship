using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHealthSystem>(out var other))
        {
            other.Death(false);
            return;
        }

        if (collision.TryGetComponent<ObjectPooler>(out var objectPooler))
        {
            ObjectPoolFactory.ReturnToPool(objectPooler);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
