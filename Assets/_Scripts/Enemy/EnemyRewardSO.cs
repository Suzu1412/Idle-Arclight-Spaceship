using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRewardSO", menuName = "Scriptable Objects/Drop/EnemyRewardSO")]
public class EnemyRewardSO : ScriptableObject
{
    [SerializeField] private int _baseCurrencyReward;

    public int BaseCurrencyReward => _baseCurrencyReward;
}
