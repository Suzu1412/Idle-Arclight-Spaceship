using UnityEngine;

[CreateAssetMenu(fileName = "BaseRandomEventSO", menuName = "Scriptable Objects/BaseRandomEventSO")]
public abstract class BaseRandomEventSO : ScriptableObject
{
    [SerializeField] protected WeightedItem WeightedItem;
    [SerializeField] protected float _duration;

    public float Duration => _duration;

    public int Weight => WeightedItem.Weight;

    public abstract void ActivateEvent();

    public abstract void DeactivateEvent();
}
