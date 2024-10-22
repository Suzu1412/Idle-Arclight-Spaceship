using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlacementStrategySO", menuName = "Scriptable Objects/Spawner/DefaultPlacementStrategySO")]
public class DefaultPlacementStrategySO : BasePlacementStrategySO
{
    public override Vector3 SetPosition(Vector3 origin)
    {
        return origin;
    }
}
