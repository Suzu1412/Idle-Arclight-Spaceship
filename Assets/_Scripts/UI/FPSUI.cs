using UnityEngine;
using UnityEngine.UI;

public class FPSUI : MonoBehaviour
{
    [SerializeField] private IntGameEvent OnFPSChangeEvent; // Send Event to FrameRateManager
    [SerializeField] private IntVariableSO _targetFramerate;
    [SerializeField] private Toggle _toggle30FPS;
    [SerializeField] private Toggle _toggle60FPS;
    [SerializeField] private Toggle _toggleMaxFPS;

    public void ChangeFPS(int amount)
    {
        OnFPSChangeEvent.RaiseEvent(amount);
        UpdateToggle();
    }

    private void OnEnable()
    {
        UpdateToggle();
    }

    private void UpdateToggle()
    {
        _toggle30FPS.SetIsOnWithoutNotify(_targetFramerate.Value == 30);
        _toggle60FPS.SetIsOnWithoutNotify(_targetFramerate.Value == 60);
        _toggleMaxFPS.SetIsOnWithoutNotify(_targetFramerate.Value == -1);
    }
}
