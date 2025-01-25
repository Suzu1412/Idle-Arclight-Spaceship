using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/RunTime Set/Saveable Runtime Set", fileName = "Saveable RuntimeSet")]
public class SaveableRunTimeSetSO : RuntimeSetSO<ISaveable>
{
    public override void Add(ISaveable item)
    {
        base.Add(item);
    }
}
