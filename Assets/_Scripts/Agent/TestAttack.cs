using UnityEngine;

public class TestAttack : MonoBehaviour
{
    private IAgent _agent;
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    [SerializeField] private HealthSystem _enemyHealth;

    private void OnEnable()
    {
        Agent.Input.OnTouchPressed += InflictDamage;
    }

    private void OnDisable()
    {
        Agent.Input.OnTouchPressed -= InflictDamage;
    }

    private void InflictDamage(bool isPressed)
    {
        if (_enemyHealth != null)
        {
            if (!isPressed)
            {
                return;
            }
            _enemyHealth.Damage((int)Agent.GetStat(StatType.Strength));
        }
        else
        {
            Debug.LogError($"Test Attack Requires Enemy");
        }
    }
}
