using UnityEngine;

[CreateAssetMenu(fileName = "RandomVerticalPlacementStrategySO", menuName = "Scriptable Objects/Spawner/RandomVerticalPlacementStrategySO")]
public class RandomVerticalPlacementStrategySO : BasePlacementStrategySO
{
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _maxPositionY;

    public override Vector3 SetPosition(Vector3 origin)
    {
        origin.y = Random.Range(_minPositionY, _maxPositionY);
        return origin;
    }
}
