using System.Collections;
using UnityEngine;

public class TestAutoAttack : MonoBehaviour
{
    private IAgent _agent;
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();
    [SerializeField] private float _delay;

    [SerializeField] private HealthSystem _enemyHealth;

    private void OnEnable()
    {
        StartCoroutine(InflictDamage());
    }

    private IEnumerator InflictDamage()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delay);

            Agent.AttackSystem.Attack(true);
        }
        
    }
}
