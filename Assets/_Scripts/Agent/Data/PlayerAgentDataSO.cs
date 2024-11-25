using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "PlayerAgentDataSO", menuName = "Scriptable Objects/Agent/PlayerAgentDataSO")]
public class PlayerAgentDataSO : AgentDataSO
{
    [SerializeField] private float _totalExp;

    [SerializeField] private bool _isUnlocked;

    public float TotalExp { get => _totalExp; internal set => _totalExp = value; }

    public bool IsUnlocked { get => _isUnlocked; internal set => _isUnlocked = value; }
}
