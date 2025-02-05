using System;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Objects/Attack/Projectile Data")]
public class ProjectileDataSO : ScriptableObject
{
    [SerializeField] private SpriteAtlas _atlas;
    [SerializeField] private string _spriteName;
    [SerializeField][Range(5, 20f)] private float _projectileLifeTime;
    [SerializeField][Range(1, 5)] private int _projectileDuration = 1;

    public void Initialize(Projectile projectile, Agent agent, Vector2 direction, Vector3 position, float speed)
    {
        projectile.SetSprite(_atlas.GetSprite(_spriteName));
        projectile.SetLifeTime(_projectileLifeTime);
        projectile.SetProjectileSpeed(speed);
        projectile.SetLayerMask(agent.AttackSystem.ProjectileMask);
        projectile.SetProjectileDamage(agent.GetStat<AttackStatSO>().Value);
        projectile.SetProjectileDuration(_projectileDuration);
        projectile.transform.right = direction;
        projectile.transform.position = position;
        projectile.RB.position = position;
    }
}
