using UnityEngine;

[CreateAssetMenu(fileName = "WaitUntilSO", menuName = "Scriptable Objects/Variable/WaitUntilSO")]
public class WaitUntilSO : ScriptableObject
{
    [SerializeField] private BoolVariableSO _isPaused;
    private WaitUntil _waitUntil;

    public WaitUntil WaitUntil => _waitUntil;

    /// <summary>
    /// Should be initialized only on the Pause Manager. if its not initialized it will give error
    /// </summary>
    public void Initialize()
    {
        _waitUntil = new WaitUntil(() => !_isPaused.Value);
    }
}
