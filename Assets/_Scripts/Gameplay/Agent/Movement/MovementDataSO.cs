using UnityEngine;

[CreateAssetMenu(fileName = "MovementDataSO", menuName = "Scriptable Objects/Stats/MovementDataSO")]
public class MovementDataSO : ScriptableObject, IMoveData
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 5f;

    public float Acceleration => _acceleration;
    public float Deceleration => _deceleration;
}
