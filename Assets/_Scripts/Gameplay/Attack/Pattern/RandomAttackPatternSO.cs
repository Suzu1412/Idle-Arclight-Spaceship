using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "RandomAttackPatternSO", menuName = "Scriptable Objects/Attack/Pattern/RandomAttackPatternSO")]

public class RandomAttackPatternSO : AttackPatternSO
{
    [SerializeField][Range(1, 50)] protected int _bulletsPerShot;
    [SerializeField][Range(0.5f, 10f)] protected float _minProjectileSpeed;
    [SerializeField][Range(0.5f, 10f)] protected float _maxProjectileSpeed;

    public override IEnumerator Execute(Transform spawnPoint, AttackSystem executor, Agent agent)
    {
        for (int i = 0; i < _repetitions; i++)
        {
            yield return executor.StartCoroutine(ExecuteNestedCoroutine(spawnPoint, agent, 0f, _fireRate));
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
            float randomAngle = Random.Range(0f, 360f);

            // Calculate the angle for this bullet
            float currentAngle = randomAngle + (360f / _bulletsPerShot) * j;

            // Create the direction vector from the angle
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0).normalized;

            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            float speed = Random.Range(_minProjectileSpeed, _maxProjectileSpeed);
            _projectileData.Initialize(bullet, agent, direction, spawnPoint.position, speed);
            _projectileSFX.PlayEvent();

        }
    }
}
