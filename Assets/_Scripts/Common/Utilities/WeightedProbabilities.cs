using System.Collections.Generic;
using UnityEngine;

public static class WeightedProbabilities
{
    /// <summary>
    /// Calculate Weight to Probability
    /// </summary>
    /// <param name="weight"></param>
    /// <param name="totalWeight"></param>
    /// <returns></returns>
    public static float CalculateProbability(int weight, int totalWeight)
    {
        return weight / totalWeight * 100;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="items">The Items list must be Ordered to work correctly</param>
    /// <param name="totalWeight"></param>
    /// <returns></returns>
    public static int GetWeightedItemList(List<int> items, int totalWeight = 0)
    {
        if (totalWeight == 0)
        {
            totalWeight = GetTotalWeight(items);
        }

        int randomWeight = Random.Range(0, totalWeight);

        for (int i = 0; i < items.Count; i++)
        {
            randomWeight -= items[i];

            if (randomWeight < 0)
            {
                return i;
            }
        }

        return 0;
    }

    /// <summary>
    /// Better to Cache if values remain the same
    /// </summary>
    /// <returns></returns>
    public static int GetTotalWeight(List<int> items)
    {
        int totalWeight = 0;

        foreach (var item in items)
        {
            totalWeight = item;
        }

        return totalWeight;
    }

}
