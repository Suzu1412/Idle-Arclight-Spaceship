using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private DropTableSO _dropTable;
    [SerializeField] private RandomCirclePlacementStrategySO _randomCirclePlacementStrategy;
    private Vector2 _placement;
    private IHealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<IHealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDeath += SpawnItemPickUp;
    }

    private void OnDisable()
    {
        healthSystem.OnDeath -= SpawnItemPickUp;
    }

    public void SetDropTable(DropTableSO dropTable)
    {
        _dropTable = dropTable;
    }

    public void SpawnItemPickUp()
    {
        List<ObjectPoolSettingsSO> dropItems = _dropTable.GetDrop();

        foreach (ObjectPoolSettingsSO item in dropItems)
        {
            ItemPickUp pickUp = ObjectPoolFactory.Spawn(item).GetComponent<ItemPickUp>();
            _placement = _randomCirclePlacementStrategy.SetPosition(transform.position);
            if (pickUp.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.position = _placement;
            }
            pickUp.transform.SetPositionAndRotation(_placement, Quaternion.identity);
        }
    }
}
