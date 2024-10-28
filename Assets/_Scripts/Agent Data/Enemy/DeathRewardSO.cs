using UnityEngine;

[CreateAssetMenu(fileName = "DeathReward", menuName = "Scriptable Objects/Drop/DeathRewardSO")]
public class DeathRewardSO : ScriptableObject
{
    [SerializeField] private double _baseCurrencyReward;
    [SerializeField] private float _baseExpReward;

    public double BaseCurrencyReward => _baseCurrencyReward;
    public float BaseExpReward => _baseExpReward;
}
