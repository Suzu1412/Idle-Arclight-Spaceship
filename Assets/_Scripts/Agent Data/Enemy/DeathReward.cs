using UnityEngine;

public class DeathReward : MonoBehaviour
{
    [SerializeField] private DeathRewardSO _reward;
    [SerializeField] private DoubleGameEvent OnGainCurrencyEvent = default;
    [SerializeField] private FloatGameEvent OnGainExpEvent = default;
    // TODO: Add Quest and Drop Reward
    private ObjectPoolSettingsSO _expPool;
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

    public void SetReward(DeathRewardSO reward)
    {
        _reward = reward;
    }

    private void GrantReward()
    {
        if (_reward == null)
        {
            Debug.Log($"{this.gameObject} has no reward attached");
            return;
        }

        Debug.Log("Giving Reward!");

        OnGainCurrencyEvent.RaiseEvent(_reward.BaseCurrencyReward);
        OnGainExpEvent.RaiseEvent(_reward.BaseExpReward);
    }
}
