using UnityEngine;

[CreateAssetMenu(fileName = "Item Movement", menuName = "Scriptable Objects/Item/Item Movement")]
public class ItemMovementSO : ScriptableObject
{
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 5f;

    public float MaxSpeed => _maxSpeed;
    public float Acceleration => _acceleration;
    public float Deceleration => _deceleration;
}
