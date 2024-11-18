using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/List Upgrade Event Listener", fileName = "List Upgrade Event Listener")]
public class ListUpgradeGameEventListener : BaseGameEventListener<List<BaseUpgradeSO>>
{
    [SerializeField] protected ListUpgradeGameEvent OnEvent = default;

    public override void Register(Action<List<BaseUpgradeSO>> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<List<BaseUpgradeSO>> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}
