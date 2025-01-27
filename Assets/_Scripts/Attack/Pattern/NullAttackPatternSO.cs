using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NullAttackPatternSO", menuName = "Scriptable Objects/Attack/Pattern/NullAttackPatternSO")]
public class NullAttackPatternSO : AttackPatternSO
{
    public override IEnumerator Execute(Transform spawnPoint, AttackSystem executor, Agent agent)
    {
        yield return null;
    }


    protected override IEnumerator ExecuteNestedCoroutine(Transform spawnPoint, Agent agent, float angle, float duration)
    {
        yield return null;
    }
}
