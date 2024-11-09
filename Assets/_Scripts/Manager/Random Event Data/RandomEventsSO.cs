using System.Collections.Generic;
using UnityEngine;

public class RandomEventsSO : ScriptableObject
{
    [SerializeReference] [SubclassSelector] private List<IRandomEvent> _randomEvents;

    public List<IRandomEvent> RandomEvents => _randomEvents;
}
