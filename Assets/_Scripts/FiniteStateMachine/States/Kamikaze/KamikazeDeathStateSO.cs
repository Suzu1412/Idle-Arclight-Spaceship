using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeDeathStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeDeathStateSO")]
public class KamikazeDeathStateSO : StateSO<KamikazeDeathStateContext>
{
    [SerializeField] private GameObjectRuntimeSetSO _enemyRTS;
    public GameObjectRuntimeSetSO EnemyRTS => _enemyRTS;
}

[System.Serializable]
public class KamikazeDeathStateContext : StateContext<KamikazeDeathStateSO>
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.Death();
    }

    public override void OnExit()
    {
        base.OnExit();
        State.EnemyRTS.Remove(_fsm.gameObject);
    }

    protected override void HandleMovement(Vector2 direction)
    {
    }

    protected override void HandleAttack(bool isAttacking)
    {
    }

    public override float EvaluateUtility()
    {
        return Agent.HealthSystem.IsDeath ? State.HighestUtility : 0f;
    }
}