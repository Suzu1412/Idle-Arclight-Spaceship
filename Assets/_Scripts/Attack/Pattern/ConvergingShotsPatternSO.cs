using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConvergingShotsPatternSO", menuName = "Scriptable Objects/Attack/Pattern/ConvergingShotsPatternSO")]
public class ConvergingShotsPatternSO : AttackPatternSO
{
    [SerializeField][Range(0.1f, 0.3f)] private float _separationX; // Horizontal distance between bullets
    [SerializeField][Range(1, 10)] private int _bulletsPerShot; // Number of bullets per attack


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

        // Calculate the starting point of the pattern
        float totalWidth = _separationX * (_bulletsPerShot - 1);
        float startX = -totalWidth / 2;

        for (int j = 0; j < _bulletsPerShot; j++)
        {
            // Calculate the position of each bullet
            Vector3 bulletPosition = spawnPoint.position + new Vector3(startX + j * _separationX, 0f, 0);

            Vector2 direction;

            if (agent.TargetDetector.TargetTransform == null)
            {
                direction = agent.FacingDirection;
            }
            else
            {
                direction = agent.transform.position.GetDirectionTo(agent.TargetDetector.TargetTransform.position);
            }

            // Instantiate the bullet
            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet, agent, direction, bulletPosition, _projectileSpeed);
        }
    }
}
