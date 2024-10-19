using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesSO", menuName = "Scriptable Objects/GameRulesSO")]
public class GameRulesSO : ScriptableObject
{
    [SerializeField] private Vector2 _levelBoundaries;

    public Vector2 LevelBoundaries => _levelBoundaries;
}
