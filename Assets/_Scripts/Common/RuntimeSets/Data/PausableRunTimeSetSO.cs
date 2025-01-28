using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/RunTime Set/Pausable Runtime Set", fileName = "Pausable RuntimeSet")]
public class PausableRunTimeSetSO : RuntimeSetSO<IPausable>
{
    [SerializeField] private BoolVariableSO _pausable;

    public override void Add(IPausable item)
    {
        base.Add(item);
        if (item.IsPaused == null)
        {
            item.IsPaused = _pausable;
        }
    }
}
