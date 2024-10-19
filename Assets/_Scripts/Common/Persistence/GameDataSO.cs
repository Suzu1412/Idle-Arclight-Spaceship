using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Scriptable Objects/Persistence/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name => _name;
    public GameData GameData;
}
