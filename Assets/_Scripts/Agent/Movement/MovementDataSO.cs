using UnityEngine;

[CreateAssetMenu(fileName = "MovementDataSO", menuName = "Scriptable Objects/Stats/MovementDataSO")]
public class MovementDataSO : ScriptableObject, IMoveData
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _maxSpeed = 7f; // Get from Stats

    public float Acceleration => _acceleration;
    public float Deceleration => _deceleration;
    public float MaxSpeed => _maxSpeed;
}
