using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttackStrategySO", menuName = "Scriptable Objects/BaseAttackStrategySO")]
public abstract class BaseAttackStrategySO : ScriptableObject
{
    public abstract BaseAttackStrategy CreateAttack();
}

public abstract class BaseAttackStrategySO<T> : 
    BaseAttackStrategySO where T : BaseAttackStrategy, new()
{
    public override BaseAttackStrategy CreateAttack() => new T();
} 
