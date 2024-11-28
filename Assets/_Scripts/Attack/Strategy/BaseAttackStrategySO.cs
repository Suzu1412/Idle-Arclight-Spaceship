using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttackStrategySO", menuName = "Scriptable Objects/BaseAttackStrategySO")]
public abstract class BaseAttackStrategySO : ScriptableObject
{
    [SerializeField] protected SoundDataSO _projectileSFX;
    [SerializeField] protected float _attackDelay = 0.2f;
    public SoundDataSO ProjectileSFX => _projectileSFX;

    public float AttackDelay => _attackDelay;
    public abstract BaseAttackStrategy CreateAttack();


}

public abstract class BaseAttackStrategySO<T> :
    BaseAttackStrategySO where T : BaseAttackStrategy, new()
{
    public override BaseAttackStrategy CreateAttack() => new T();
}
