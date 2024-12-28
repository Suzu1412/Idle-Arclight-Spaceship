using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable;
    private Rigidbody2D _rb;
    [SerializeField] private LayerMask whatDestroyBullet;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private float _projectileRange = 10f;
    [SerializeField] private float _damage = 5;
    private SpriteRenderer spriteRenderer;
    private ObjectPooler _pool;
    [SerializeField] private SoundDataSO _impactSFX;
    [SerializeField] private ObjectPoolSettingsSO _impactPool = default;
    [SerializeField] private float projectileDuration = 6f;
    public float ProjectileRange => _projectileRange;
    private Coroutine _disableProjectileAfterDistance;
    private bool _isPaused;

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
        _pausable.Add(this);
    }

    private void OnDisable()
    {
        _pausable.Remove(this);
    }

    private void FixedUpdate()
    {
        // Rotate Bullet in direction of velocity
        if (_isPaused) return;
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

    public void SetLayerMask(LayerMask layerMask)
    {
        this.gameObject.layer = layerMask.GetLayer();
    }

    public void UpdateWeaponInfo(float projectileRange)
    {
        _projectileRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            hittable.GetHit(this.gameObject);
        }

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(Mathf.RoundToInt(_damage));
            SpawnImpactEffect();
            _impactSFX.PlayEvent();
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

    public void Pause(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            RB.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            RB.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
