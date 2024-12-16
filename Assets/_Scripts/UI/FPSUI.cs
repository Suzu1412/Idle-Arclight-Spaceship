using UnityEngine;

public class FPSUI : MonoBehaviour
{
    [SerializeField] private FloatGameEvent OnFPSChangeEvent;

    public void ChangeFPS(float amount)
    {
        OnFPSChangeEvent.RaiseEvent(amount);
    }
}
