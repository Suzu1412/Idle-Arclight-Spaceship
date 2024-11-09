using UnityEngine;

[System.Serializable]
public class WeightedItem
{
    [SerializeField] [Range(1, 1000)] private int _weight = 1;
    [SerializeField] [ReadOnly] private float _probabilityChance = 0;


    public int Weight => _weight;

    public void CalculateProbability(int totalWeight)
    {
        _probabilityChance = WeightedProbabilities.CalculateProbability(_weight, totalWeight);
    }


}
