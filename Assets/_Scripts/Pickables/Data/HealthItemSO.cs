using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DropItem/Health Drop", fileName = "New Health Drop")]
public class HealthItemSO : ItemSO
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private bool isStorable;

    public override void PickUp(IAgent agent)
    {
        //agent.HealthSystem.Heal(healAmount);
    }
}
    
