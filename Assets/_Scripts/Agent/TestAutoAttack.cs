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

            if (_enemyHealth != null)
            {
                _enemyHealth.GetHit(this.gameObject);
                _enemyHealth.Damage((int)Agent.GetStat(StatType.Strength));
            }
            else
            {
                Debug.LogError($"Test Auto Attack Requires Enemy");
            }
        }
        
    }
}
