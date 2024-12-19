using UnityEngine;

[CreateAssetMenu(fileName = "RandomHorizontalPlacementStrategySO", menuName = "Scriptable Objects/Spawner/RandomHorizontalPlacementStrategySO")]
public class RandomHorizontalPlacementStrategySO : BasePlacementStrategySO
{
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _maxPositionX;

    public override Vector3 SetPosition(Vector3 origin)
    {
        origin.x = Random.Range(_minPositionX, _maxPositionX);
        return origin;
    }
}
