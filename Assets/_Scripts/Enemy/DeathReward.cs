using UnityEngine;

public class DeathReward : MonoBehaviour
{
    [SerializeField] private EnemyRewardSO _reward;
    [SerializeField] private DoubleGameEvent _gainCurrencyEvent = default;
    private IHealthSystem _healthSystem;

    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();

    private void OnEnable()
    {
        HealthSystem.OnDeath += GrantReward;
    }

    private void OnDisable()
    {
        HealthSystem.OnDeath -= GrantReward;
    }

    private void GrantReward()
    {
        if (_reward == null)
        {
            Debug.Log($"{this.gameObject} has no reward attached");
            return;
        }
        _gainCurrencyEvent.RaiseEvent(_reward.BaseCurrencyReward);
    }
}
