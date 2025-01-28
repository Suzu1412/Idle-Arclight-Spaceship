using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemSO : ItemSO
{
    [SerializeField][Range(1, 99)] private int itemAmount = 1;
    [SerializeField][Range(10, 255)] private int itemMaxAmount = 99;

    public int ItemAmount => itemAmount;
    public int ItemMaxAmount => itemMaxAmount;


    public override void PickUp(IAgent agent)
    {
        // Add to Inventory 
    }
}
