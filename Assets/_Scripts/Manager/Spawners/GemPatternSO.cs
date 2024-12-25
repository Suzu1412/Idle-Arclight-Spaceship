using UnityEngine;

[CreateAssetMenu(fileName = "GemPatternSO", menuName = "Scriptable Objects/GemPatternSO")]
public class GemPatternSO : ScriptableObject
{
    [SerializeField] private GameObject _gemPattern;

    public GameObject GemPattern => _gemPattern;
}
