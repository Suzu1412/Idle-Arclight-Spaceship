using UnityEngine;
using System.Collections.Generic;

public class RandomEventManager : Singleton<RandomEventManager>
{
    private List<int> _weightedItems = new();
    private int _totalWeight = 0;

    [SerializeField] private List<BaseRandomEventSO> _randomEvents;

    [Header("Random Event Game Event")]
    [SerializeField] private RandomEventGameEvent OnNotificateRandomEvent;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnActivateRandomEventListener;



    private void OnEnable()
    {
        OnActivateRandomEventListener.Register(ActivateRandomEvent);
    }

    private void OnDisable()
    {
        OnActivateRandomEventListener.DeRegister(ActivateRandomEvent);
        StopRandomEvents();
    }

    private void Start()
    {
        PopulateWeightList();
        ActivateRandomEvent();
    }

    private async void ActivateRandomEvent()
    {
        int index = WeightedProbabilities.GetWeightedItemList(_weightedItems, _totalWeight);
        _randomEvents[index].ActivateEvent();
        OnNotificateRandomEvent.RaiseEvent(_randomEvents[index]);
        await Awaitable.WaitForSecondsAsync(_randomEvents[index].Duration);
        _randomEvents[index].DeactivateEvent();

    }

    private void PopulateWeightList()
    {
        if (_weightedItems.IsNullOrEmpty())
        {
            _weightedItems = new();
        }

        for (int i = 0; i < _randomEvents.Count; i++)
        {
            _weightedItems.Add(_randomEvents[i].Weight);
        }

        _totalWeight = WeightedProbabilities.GetTotalWeight(_weightedItems);

        foreach (var item in _randomEvents)
        {
            item.SetTotalWeight(_totalWeight);
        }
    }

    private void StopRandomEvents()
    {
        foreach (var item in _randomEvents)
        {
            item.DeactivateEvent();
        }
    }
}
