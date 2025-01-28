using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Health Drop", fileName = "New Health Drop")]
public class HealthItemSO : ItemSO
{
    [SerializeField] [Range(0f, 1f)] private float healAmountPercent = 1;
    [SerializeField] private bool isStorable;

    public override void PickUp(IAgent agent)
    {
        agent.HealthSystem.Heal(Mathf.RoundToInt(agent.HealthSystem.GetMaxHealth() * healAmountPercent));
    }
}
    
