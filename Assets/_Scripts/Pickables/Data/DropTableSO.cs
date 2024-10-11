using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DropItem/Drop table", fileName = "New Drop Table")]
public class DropTableSO : ScriptableObject
{
    [SerializeField] private List<DropDefinition> _drops;

    public List<ItemSO> GetDrop()
    {
        List<ItemSO> dropItems = new();

        foreach (DropDefinition drop in _drops)
        {
            bool shouldDrop = Random.value < drop.DropChance;
            if (shouldDrop)
            {
                dropItems.Add(drop.Item);
            }
        }

        return dropItems;
    }
}
