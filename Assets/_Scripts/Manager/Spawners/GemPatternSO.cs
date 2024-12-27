using UnityEngine;

[CreateAssetMenu(fileName = "GemPatternSO", menuName = "Scriptable Objects/GemPatternSO")]
public class GemPatternSO : ScriptableObject
{
    [SerializeField] private ObjectPoolSettingsSO[] _gemPatternPools;

    public ObjectPoolSettingsSO[] GemPatternPools => _gemPatternPools;
}
