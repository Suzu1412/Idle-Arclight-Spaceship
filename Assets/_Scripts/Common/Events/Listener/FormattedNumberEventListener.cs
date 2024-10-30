using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Listener/FormattedNumber Event Listener", fileName = "FormattedNumber Event Listener")]

public class FormattedNumberEventListener : BaseGameEventListener<FormattedNumber>
{
    [SerializeField] protected FormattedNumberGameEvent OnEvent = default;

    public override void Register(Action<FormattedNumber> onEvent)
    {
        OnEvent.Add(onEvent);
    }

    public override void DeRegister(Action<FormattedNumber> onEvent)
    {
        OnEvent.Remove(onEvent);
    }
}
