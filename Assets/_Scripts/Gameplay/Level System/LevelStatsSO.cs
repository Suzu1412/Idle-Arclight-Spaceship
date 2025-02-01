using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Level", fileName = "Level Stats")]
public class LevelStatsSO : ScriptableObject
{
    [SerializeField] private LevelStatModifier[] _levelStats = new LevelStatModifier[99];
    public LevelStatModifier[] LevelStats => _levelStats;

    public void AddModifiers(int previousLevel, int currentLevel, IAgent agent)
    {
        for(int i = previousLevel; i < currentLevel; i++)
        {
            foreach (var statModifier in LevelStats[i].StatsModifiers)
            {
                agent.StatsSystem.AddModifier(statModifier);
            }
        }
    }

    public void RemoveModifiers(int currentLevel, IAgent agent)
    {
        for (int i = 0 - 1; i < currentLevel; i++)
        {
            foreach (var statModifier in LevelStats[i].StatsModifiers)
            {
                agent.StatsSystem.RemoveModifier(statModifier);
            }
        }
    }
    
}
