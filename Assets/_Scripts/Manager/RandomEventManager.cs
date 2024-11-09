using UnityEngine;
using System.Collections.Generic;

public class RandomEventManager : Singleton<RandomEventManager>
{
    private List<int> _weightedItems = new();
    private int _totalWeight = 0;

    [SerializeField] private RandomEventsSO _randomEvents;

    [Header("Void Event")]
    [SerializeField] private VoidGameEventListener OnActivateRandomEventListener;

    private void OnEnable()
    {
        OnActivateRandomEventListener.Register(ActivateRandomEvent);
    }

    private void OnDisable()
    {
        OnActivateRandomEventListener.DeRegister(ActivateRandomEvent);
    }

    private void Start()
    {
        PopulateWeightList();
        ActivateRandomEvent();
    }

    private void ActivateRandomEvent()
    {
        int index = WeightedProbabilities.GetWeightedItemList(_weightedItems, _totalWeight);
        _randomEvents.RandomEvents[index].ActivateEvent();
    }

    private void PopulateWeightList()
    {
        if (_weightedItems.IsNullOrEmpty())
        {
            _weightedItems = new();
        }

        for (int i = 0; i < _randomEvents.RandomEvents.Count; i++)
        {
            _weightedItems.Add(_randomEvents.RandomEvents[i].WeightedItem.Weight);
        }

        _totalWeight = WeightedProbabilities.GetTotalWeight(_weightedItems);
    }
}
