using UnityEngine;

public class GlowMaterialFeedback : Feedback
{
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _redMaterial;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();
    private SpriteRenderer _sprite;
    internal SpriteRenderer Sprite => _sprite != null ? _sprite : _sprite = GetComponent<SpriteRenderer>();

    private void OnEnable()
    {
        HealthSystem.OnHit += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnHit -= StartFeedback;
        ResetFeedback();
    }

    public override void ResetFeedback()
    {
        Sprite.material = _blueMaterial;
    }

    public override void StartFeedback()
    {
        float ratio = HealthSystem.GetHealthPercent();

        if (ratio > 0.67f && ratio <= 1f)
        {
            Sprite.material = _blueMaterial;
        }
        else if (ratio > 0.34f && ratio <= 0.67f)
        {
            Sprite.material = _yellowMaterial;
        }
        else
        {
            Sprite.material = _redMaterial;
        }

    }
}
