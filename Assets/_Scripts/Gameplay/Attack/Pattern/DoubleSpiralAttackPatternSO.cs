using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleSpiralAttackPatternSO", menuName = "Scriptable Objects/Attack/Pattern/DoubleSpiralAttackPatternSO")]
public class DoubleSpiralAttackPatternSO : AttackPatternSO
{
    [SerializeField][Range(1, 25)] protected int _bulletsPerShot;
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
            // First Projectile
            // Calculate the angle for this bullet
            float currentAngle = baseAngle + angle + (360f / _bulletsPerShot) * j;

            // Create the direction vector from the angle
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0).normalized;

            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet, agent, direction, spawnPoint.position, _projectileSpeed);

            // Second Projectile
            // Calculate the angle for this bullet
            float currentAngle2 = baseAngle + angle + (360f / _bulletsPerShot) * j; // Rotate in opposite direction

            // Create the direction vector from the angle
            Vector3 direction2 = new Vector3(Mathf.Cos(currentAngle2 * Mathf.Deg2Rad), Mathf.Sin(-currentAngle2 * Mathf.Deg2Rad), 0).normalized;

            Projectile bullet2 = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet2, agent, direction2, spawnPoint.position, _projectileSpeed);
            _projectileSFX.PlayEvent();

        }
    }
}
