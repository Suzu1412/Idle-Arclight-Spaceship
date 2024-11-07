using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [Header("Float Event Listener")]
    [SerializeField] private FloatGameEventListener OnLoadProgressEventListener;

    private void OnEnable()
    {
        OnLoadProgressEventListener.Register(UpdateUIProgressBar);
    }

    private void OnDisable()
    {
        OnLoadProgressEventListener.DeRegister(UpdateUIProgressBar);
    }

    private void UpdateUIProgressBar(float value)
    {
        Debug.Log(value);
    }
}
