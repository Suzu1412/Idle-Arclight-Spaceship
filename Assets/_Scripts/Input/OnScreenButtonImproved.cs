using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenButtonImproved : OnScreenControl, IPointerClickHandler, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData data)
    {
        SendValueToControl(0.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SendValueToControl(1.0f);
    }

    [InputControl(layout = "Button")]
    [SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}
