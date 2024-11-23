using System.Collections.Generic;
using UnityEngine;

public class ListPlayerAgentDataSO : ScriptableObject
{
    [SerializeField] private List<PlayerAgentDataSO> _players;

    public List<PlayerAgentDataSO> Players => _players;
}
