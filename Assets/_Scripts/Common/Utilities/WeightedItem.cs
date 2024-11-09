using UnityEngine;

[System.Serializable]
public class WeightedItem
{
    [SerializeField] [Range(1, 1000)] private int _weight = 1;
    [SerializeField] [ReadOnly] private int _totalWeight;
    [SerializeField] [ReadOnly] private float _probabilityChance = 0;

    public int Weight => _weight;
    public int TotalWeight => _totalWeight;

    public void SetTotalWeight(int totalWeight)
    {
        _totalWeight = totalWeight;
    }

    public void CalculateProbability()
    {
        if (_totalWeight == 0)
        {
            Debug.LogError($"{_totalWeight} not set in {this}");
        }
        _probabilityChance = WeightedProbabilities.CalculateProbability(_weight, _totalWeight);
    }


}
