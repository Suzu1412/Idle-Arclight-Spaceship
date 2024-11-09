using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Random Events", fileName = "Random Events")]
public class RandomEventsSO : ScriptableObject
{
    [SerializeReference] [SubclassSelector] private List<IRandomEvent> _randomEvents;

    public List<IRandomEvent> RandomEvents => _randomEvents;
}
