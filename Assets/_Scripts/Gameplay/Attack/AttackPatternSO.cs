using System.Collections;
using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{
    [SerializeField] protected BoolVariableSO _isPaused;
    [SerializeField] protected ObjectPoolSettingsSO _projectilePool;
    [SerializeField] protected ProjectileDataSO _projectileData; // Set Projectile Sprite and other attributes
    [SerializeField] protected SoundDataSO _projectileSFX;
    [SerializeField][Range(0.5f, 10f)] protected float _projectileSpeed = 1f;
    [SerializeField][Range(0.05f, 1f)] protected float _fireRate = 0.2f; // Time between shots
    [SerializeField][Range(0.05f, 10f)] protected float _cooldown = 0.2f; // Time before it can be activated again
    [SerializeField][Range(1, 36)] protected int _repetitions; // Number of shots to fire

    public SoundDataSO ProjectileSFX => _projectileSFX;
    public int Repetitions => _repetitions;
    public float FireRate => _fireRate;
    public float Cooldown => _cooldown;

    public abstract IEnumerator Execute(Transform spawnPoint, AttackSystem executor, Agent agent);

    protected abstract IEnumerator ExecuteNestedCoroutine(Transform spawnPoint, Agent agent, float angle, float duration);
}
