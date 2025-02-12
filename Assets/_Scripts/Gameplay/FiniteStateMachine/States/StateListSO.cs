using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StateListSO", menuName = "Scriptable Objects/FSM/StateListSO")]
public class StateListSO : ScriptableObject
{
    [SerializeField] private StateSO _defaultState;
    [SerializeField] private List<StateSO> _states;

    public StateSO DefaultState => _defaultState;

    public List<StateSO> GetStates() => _states;
}
