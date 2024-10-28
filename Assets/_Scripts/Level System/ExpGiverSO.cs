using UnityEngine;

[CreateAssetMenu(fileName = "ExpGiverSO", menuName = "Scriptable Objects/Reward/ExpGiverSO")]
public class ExpGiverSO : ScriptableObject
{
    [SerializeField] private FloatGameEvent OnExpGainEvent;


}
