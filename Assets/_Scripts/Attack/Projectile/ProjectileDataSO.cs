using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Objects/Attack/Projectile Data")]
public class ProjectileDataSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] [Range(5, 20f)] private float _projectileLifeTime;

    public void Initialize(Projectile projectile, Agent agent, Vector2 direction, Vector3 position, float speed)
    {
        projectile.SetSprite(_sprite);
        projectile.SetLifeTime(_projectileLifeTime);
        projectile.SetProjectileSpeed(speed);
        projectile.SetLayerMask(agent.AttackSystem.ProjectileMask);
        projectile.SetProjectileDamage(agent.GetStat<AttackStatSO>().Value);
        projectile.transform.right = direction;
        projectile.transform.position = position;
        projectile.RB.position = position;
    }
}
