using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private DropTableSO dropTable;
    [SerializeField] private ObjectPoolSettingsSO _itemPickUpPool;
    //private IHealthSystem healthSystem;

    private void Awake()
    {
        //healthSystem = GetComponent<IHealthSystem>();
    }

    private void OnEnable()
    {
        //healthSystem.OnDeath += SpawnItemPickUp;
    }

    private void OnDisable()
    {
        //healthSystem.OnDeath -= SpawnItemPickUp;
    }

    public void SpawnItemPickUp()
    {
        List<ItemSO> dropItems = dropTable.GetDrop();

        foreach (ItemSO item in dropItems)
        {
            ItemPickUp pickUp = ObjectPoolFactory.Spawn(_itemPickUpPool).GetComponent<ItemPickUp>();
            pickUp.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            pickUp.SetItem(item);
        }
    }
}
