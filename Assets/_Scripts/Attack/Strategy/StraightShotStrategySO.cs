using UnityEngine;

[CreateAssetMenu(fileName = "StraightShotStrategySO", menuName = "Scriptable Objects/Attack/Range/StraightShotStrategySO")]
public class StraightShotStrategySO : BaseAttackStrategySO<StraightShotStrategy>
{
    [SerializeField] protected ObjectPoolSettingsSO _projectilePool;
    public ObjectPoolSettingsSO ProjectilePool => _projectilePool;
}

public class StraightShotStrategy : BaseAttackStrategy
{
    private bool _isAttacking = false;
    protected ObjectPoolSettingsSO _projectilePool;

    public override void Initialize(IAgent agent, BaseAttackStrategySO origin, Transform spawnPosition)
    {
        base.Initialize(agent, origin, spawnPosition);
        _projectilePool = (origin as StraightShotStrategySO).ProjectilePool;
    }

    public override void Attack(bool isPressed)
    {
        if (isPressed)
        {
            Shoot();
        }
    }

    private async void Shoot()
    {
        _isAttacking = true;
        Projectile projectile = ObjectPoolFactory.Spawn(_projectilePool).GetComponent<Projectile>();
        projectile.SetProjectileSpeed(10f);
        projectile.SetProjectileDamage(_agent.GetStat(StatType.Strength));
        projectile.SetLayerMask(_agent.AttackSystem.ProjectileMask);
        projectile.transform.position = _spawnPosition.position;
        projectile.transform.right = CalculateSpreadAngle(_agent.FacingDirection);
        _projectileSFX.PlayEvent();
        await Awaitable.WaitForSecondsAsync(0.1f);
        _isAttacking = false;
    }

    private Vector2 CalculateSpreadAngle(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Vector2 trajectory = Vector2.zero;

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);

        trajectory.Set(x, y);
        return trajectory;
    }

}
