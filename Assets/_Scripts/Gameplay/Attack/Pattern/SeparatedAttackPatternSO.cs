using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SeparatedAttackPatternSO", menuName = "Scriptable Objects/Attack/Pattern/SeparatedAttackPatternSO")]
public class SeparatedAttackPatternSO : AttackPatternSO
{
    [SerializeField][Range(0.1f, 0.3f)] private float _separationX; // Horizontal distance between bullets
    [SerializeField][Range(0f, 2f)] private float _amplitude = 1f; // Height of the wave
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
            float waveOffsetY = 0f;
            if (_bulletsPerShot > 1)
            {
                waveOffsetY = Mathf.Sin(j * Mathf.PI / (_bulletsPerShot - 1)) * _amplitude; // Adjust amplitude
            }
            Vector3 bulletPosition = spawnPoint.position + new Vector3(startX + j * _separationX, waveOffsetY, 0);

            var direction = agent.FacingDirection;

            // Instantiate the bullet
            Projectile bullet = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
            _projectileData.Initialize(bullet, agent, direction, bulletPosition, _projectileSpeed);
            _projectileSFX.PlayEvent();

        }
    }
}
