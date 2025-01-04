using DamageNumbersPro;
using TMPro;
using UnityEngine;

public class DamageTextFeedback : TextPopUp
{
    [SerializeField] private DamageNumber _numberPrefab;
    private Transform _transform;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        HealthSystem.OnDamaged += SpawnPopUp;
    }

    private void OnDisable()
    {
        HealthSystem.OnDamaged -= SpawnPopUp;
    }

    protected override void SpawnPopUp(int text)
    {
        DamageNumber damageNumber = _numberPrefab.Spawn(_transform.position, "- " + text);
    }
}
