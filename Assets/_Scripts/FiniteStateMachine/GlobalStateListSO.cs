using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/State/Global State List", fileName = "Global State")]
public class GlobalStateListSO : ScriptableObject
{
    [SerializeField] private BaseStateSO _idleState;
    [SerializeField] private BaseStateSO _hurtState;
    [SerializeField] private BaseStateSO _deathState;
    [SerializeField] private BaseStateSO _battleState;
    //[SerializeField] private BaseStateSO _stunState;
    //[SerializeField] private BaseStateSO _freezeState;
    //[SerializeField] private BaseStateSO _burnState;

    public BaseStateSO IdleState => _idleState;
    public BaseStateSO HurtState => _hurtState;
    public BaseStateSO DeathState => _deathState;
    public BaseStateSO BattleState => _battleState;
}
