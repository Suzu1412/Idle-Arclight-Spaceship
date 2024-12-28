using UnityEngine;
using UnityEngine.UI;

public class FPSUI : MonoBehaviour
{
    [SerializeField] private FloatGameEvent OnFPSChangeEvent; // Send Event to FrameRateManager
    [SerializeField] private FloatVariableSO _fpsAmount;
    [SerializeField] private Toggle _toggle30FPS;
    [SerializeField] private Toggle _toggle60FPS;
    [SerializeField] private Toggle _toggleMaxFPS;

    public void ChangeFPS(float amount)
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
        _toggle30FPS.SetIsOnWithoutNotify(_fpsAmount.Value == 30);
        _toggle60FPS.SetIsOnWithoutNotify(_fpsAmount.Value == 60);
        _toggleMaxFPS.SetIsOnWithoutNotify(_fpsAmount.Value == 0);
    }
}
