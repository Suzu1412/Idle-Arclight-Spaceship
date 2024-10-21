using UnityEngine;

[CreateAssetMenu(fileName = "RandomCirclePlacementStrategySO", menuName = "Scriptable Objects/Spawner/RandomCirclePlacementStrategySO")]
public class RandomCirclePlacementStrategySO : BasePlacementStrategySO
{
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance = 10f;

    public override Vector3 SetPosition(Vector3 origin)
    {
        return origin.RandomPointInAnnulus(_minDistance, _maxDistance);
    }
}
