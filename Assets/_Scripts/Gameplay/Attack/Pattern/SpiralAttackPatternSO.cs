using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralAttackPatternSO", menuName = "Scriptable Objects/Attack/Pattern/SpiralAttackPatternSO")]
public class SpiralAttackPatternSO : AttackPatternSO
{
    [SerializeField][Range(1, 50)] protected int _bulletsPerShot;
    [SerializeField][Range(10, 90)] protected float _rotationSpeed; // Degrees per bullet

    public override IEnumerator Execute(Transform spawnPoint, AttackSystem executor, Agent agent)
    {
        float angle = 0f;

        for (int i = 0; i < _repetitions; i++)
        {
            yield return executor.StartCoroutine(ExecuteNestedCoroutine(spawnPoint, agent, angle, _fireRate));
            angle += _rotationSpeed;
        }
    }

    protected override IEnumerator ExecuteNestedCoroutine(Transform spawnPoint, Agent agent, float angle, float duration)
    {
        // Base angle of the agent's facing direction (converted to degrees)
        float baseAngle = Mathf.Atan2(agent.FacingDirection.y, agent.FacingDirection.x) * Mathf.Rad2Deg;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            while (_isPaused.Value)
            {
                yield return null; // Wait until unpaused
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int j = 0; j < _bulletsPerShot; j++)
        {
            // Calculate the angle for this bullet
            float currentAngle = baseAngle + angle + (360f / _bulletsPerShot) * j;

            // Create the direction vector from the angle
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0).normalized;

            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet, agent, direction, spawnPoint.position, _projectileSpeed);
            //bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * 5f; // Adjust speed
            _projectileSFX.PlayEvent();

        }
    }
}
