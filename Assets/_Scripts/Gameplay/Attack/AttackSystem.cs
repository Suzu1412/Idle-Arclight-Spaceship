using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackSystem : MonoBehaviour, IAttack, IPausable
{
    private Agent _agent;

    private Coroutine _attackCoroutine;
    [SerializeField] private BoolVariableSO _isPaused;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private AttackPatternSO _attackPattern;
    [SerializeField] private LayerMask _projectileMask;
    internal Agent Agent => _agent = _agent != null ? _agent : _agent = GetComponent<Agent>();
    public Transform AttackPosition => _attackPosition;

    public LayerMask ProjectileMask => _projectileMask;

    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void OnEnable()
    {
        _attackCoroutine = null;
    }

    private void Update()
    {
        if (_isPaused.Value) return;
    }

    public void Attack(bool isPressed)
    {
        if (isPressed)
        {
            if (_attackCoroutine != null)
            {
                return; // Do not start a new coroutine if one is already running
            }

            _attackCoroutine = StartCoroutine(RunAttackPattern());
        }
    }

    private IEnumerator RunAttackPattern()
    {
        yield return _attackPattern.Execute(_attackPosition, this, Agent);

        // Coroutine finished
        _attackCoroutine = null;
    }


    public void Pause(bool isPaused)
    {
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
