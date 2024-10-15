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
        Agent.AttackSystem.Attack(isPressed);
            //_enemyHealth.GetHit(this.gameObject);
            //_enemyHealth.Damage((int)Agent.GetStat(StatType.Strength));
    }
}
