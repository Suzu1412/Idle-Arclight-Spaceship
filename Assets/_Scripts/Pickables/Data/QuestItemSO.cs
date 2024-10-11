using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DropItem/Quest Item Drop", fileName = "New Quest Item Drop")]
public class QuestItemSO : ItemSO
{
    [SerializeField][Range(1, 99)] private int itemAmount;
    [SerializeField] private bool isUnique;

    public int ItemAmount => itemAmount;

    public bool IsUnique => isUnique;

    public override void PickUp(IAgent agent)
    {
        // Add to Inventory
    }
}
