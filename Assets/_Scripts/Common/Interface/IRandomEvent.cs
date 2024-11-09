using UnityEngine;

public interface IRandomEvent
{
    WeightedItem WeightedItem { get; }

    void ActivateEvent();

    void DeactivateEvent();
}
