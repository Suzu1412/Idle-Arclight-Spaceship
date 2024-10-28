using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private LayerMask whatDestroyBullet;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private float _projectileRange = 10f;
    [SerializeField] private float _damage = 5;
    private SpriteRenderer spriteRenderer;
    private ObjectPooler _pool;
    [SerializeField] private ObjectPoolSettingsSO _impactPool = default;
    [SerializeField] private float projectileDuration = 6f;
    public float ProjectileRange => _projectileRange;
    private Coroutine _disableProjectileAfterDistance;

    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    public Rigidbody2D RB => _rb != null ? _rb : _rb = gameObject.GetOrAdd<Rigidbody2D>();


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RB.gravityScale = 0f;
    }

    private void OnEnable()
    {
        _disableProjectileAfterDistance = StartCoroutine(DisableProjectileAfterDistanceCoroutine());
    }

    private void FixedUpdate()
    {
        // Rotate Bullet in direction of velocity
        RB.linearVelocity = transform.right * _projectileSpeed;
    }

    public void Initialize(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;

    }
    public void SetProjectileDamage(float damage)
    {
        _damage = damage;
    }

    public void SetProjectileSpeed(float speed)
    {
        _projectileSpeed = speed;
    }

    public void UpdateWeaponInfo(float projectileRange)
    {
        _projectileRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(Mathf.RoundToInt(_damage));
            SpawnImpactEffect();
            ObjectPoolFactory.ReturnToPool(Pool);
        }
    }

    private void SpawnImpactEffect()
    {
        Vector2 offset = Vector2.zero;
        offset.Set(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        ObjectPoolFactory.Spawn(_impactPool).transform.SetPositionAndRotation((Vector2)transform.position, transform.rotation);

    }

    private IEnumerator DisableProjectileAfterDistanceCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 target = new(transform.position.x + _projectileRange, transform.position.y);
        Vector3 offset = target - transform.position;
        float sqrLen = offset.sqrMagnitude;

        while (elapsedTime < projectileDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ObjectPoolFactory.ReturnToPool(Pool);
    }
}
