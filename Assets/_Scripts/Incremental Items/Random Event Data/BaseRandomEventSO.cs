using UnityEngine;

[CreateAssetMenu(fileName = "BaseRandomEventSO", menuName = "Scriptable Objects/BaseRandomEventSO")]
public abstract class BaseRandomEventSO : ScriptableObject
{
    [SerializeField] protected WeightedItem WeightedItem;
    [SerializeField] protected float _duration;
    [SerializeField] protected string _description;

    public float Duration => _duration;

    public int Weight => WeightedItem.Weight;

    public string Description => _description;

    public abstract void ActivateEvent();

    public abstract void DeactivateEvent();

    public void SetTotalWeight(int totalWeight)
    {
        WeightedItem.SetTotalWeight(totalWeight);
        WeightedItem.CalculateProbability();
    }
}
