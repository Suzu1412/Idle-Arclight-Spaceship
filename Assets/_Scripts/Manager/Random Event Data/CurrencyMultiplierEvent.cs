using UnityEngine;

[System.Serializable]
public class CurrencyMultiplierEvent : IRandomEvent
{
    [SerializeField] private WeightedItem _weightedItem;

    public WeightedItem WeightedItem => _weightedItem;

    public void ActivateEvent()
    {
        Debug.Log("Activando evento!");
    }

    public void DeactivateEvent()
    {
    }
}
