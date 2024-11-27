using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "UnlockedSO", menuName = "Scriptable Objects/Incremental/Unlocked System/UnlockedSO")]
public class UnlockedSystemSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private bool _isUnlocked;

    public string Name => _name;

    public string Description => _description;

    public bool IsUnlocked { get => _isUnlocked; internal set => _isUnlocked = value; }

    internal void Initialize()
    {
        IsUnlocked = false;
    }
}
