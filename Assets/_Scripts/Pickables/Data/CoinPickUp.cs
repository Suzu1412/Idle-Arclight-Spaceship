using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DropItem/Coin Drop", fileName = "New Coin Drop")]
public class CoinPickUp : ItemSO
{
    [SerializeField] private int amount;

    public override void PickUp(IAgent agent)
    {
        //agent.InventorySystem.AddCoins(amount);
    }
}