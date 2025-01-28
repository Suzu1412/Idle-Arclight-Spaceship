using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "List PlayerAgentDataSO", menuName = "Scriptable Objects/Agent/List PlayerAgentData")]
public class ListPlayerAgentDataSO : ScriptableObject
{
    [SerializeField] private List<PlayerAgentDataSO> _players;

    public List<PlayerAgentDataSO> Players => _players;
}
