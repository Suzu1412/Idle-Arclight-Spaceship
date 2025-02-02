using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RadialBurstPatternSO", menuName = "Scriptable Objects/Attack/Pattern/RadialBurstPatternSO")]
public class RadialBurstPatternSO : AttackPatternSO
{
    [SerializeField][Range(1, 50)] protected int _bulletsPerShot;
    [SerializeField][Range(1, 360)] public float _spreadAngle;  // Spread angle in degrees

    public override IEnumerator Execute(Transform spawnPoint, AttackSystem executor, Agent agent)
    {
        for (int i = 0; i < _repetitions; i++)
        {
            float baseAngle = -_spreadAngle / 2;
            yield return executor.StartCoroutine(ExecuteNestedCoroutine(spawnPoint, agent, baseAngle, _fireRate));
        }
    }

    protected override IEnumerator ExecuteNestedCoroutine(Transform spawnPoint, Agent agent, float angle, float duration)
    {
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
            float currentAngle;

            if (_bulletsPerShot == 1)
            {
                // If only one bullet, fire it directly at the base angle
                currentAngle = angle;
            }
            else
            {
                // Otherwise, calculate the angle for each bullet
                currentAngle = angle + (_spreadAngle / (_bulletsPerShot - 1)) * j;
            }

            // Create the direction vector from the angle
            Vector3 direction = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0).normalized;

            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet, agent, direction, spawnPoint.position, _projectileSpeed);
            _projectileSFX.PlayEvent();

        }
    }
}
