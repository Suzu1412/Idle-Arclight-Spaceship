using System.Collections;
using UnityEngine;

public class WhiteFlashFeedback : Feedback
{
    private SpriteRenderer _sprite;
    [SerializeField] private float _feedbackTime = 0.2f;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();


    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

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
        StopAllCoroutines();
        ToggleMaterial(1);
    }

    public override void StartFeedback()
    {        
        if (_sprite == null || _sprite.material.HasProperty("_MakeSolidColor") == false)
        {
            return;
        }

        ResetFeedback();
        StartCoroutine(ResetColor());
    }

    private void ToggleMaterial(int val)
    {
        val = Mathf.Clamp(val, 0, 1);
        _sprite.material.SetInt("_MakeSolidColor", val);
    }

    private IEnumerator ResetColor()
    {
        ToggleMaterial(0);
        yield return Helpers.GetWaitForSeconds(_feedbackTime);
        ToggleMaterial(1);
    }
}
