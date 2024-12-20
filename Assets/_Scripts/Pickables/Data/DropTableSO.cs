using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Drop table", fileName = "New Drop Table")]
public class DropTableSO : ScriptableObject
{
    [SerializeField] private List<DropDefinition> _drops;

    public List<ObjectPoolSettingsSO> GetDrop()
    {
        List<ObjectPoolSettingsSO> dropItems = new();

        foreach (DropDefinition drop in _drops)
        {
            bool shouldDrop = Random.value < drop.DropChance;
            if (shouldDrop)
            {
                dropItems.Add(drop.ItemPool);
            }
        }

        return dropItems;
    }
}
