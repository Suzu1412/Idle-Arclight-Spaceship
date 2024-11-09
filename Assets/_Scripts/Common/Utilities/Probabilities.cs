using UnityEngine;

public static class Probabilities
{
    /// <summary>
    /// Check a single probability value between 1 and 100
    /// </summary>
    /// <param name="probability">Must be Between 0 and 100</param>
    /// <returns></returns>
    public static bool CheckProbability(int probability)
    {
        float randomNumber = Random.Range(1, 101);
        if (randomNumber <= probability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
