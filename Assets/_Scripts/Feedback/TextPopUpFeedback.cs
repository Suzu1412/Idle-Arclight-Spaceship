using UnityEngine;
using UnityEngine.Pool;

public class TextPopUpFeedback : Feedback
{
    [SerializeField] private ObjectPoolSettingsSO _textData;
    private IHealthSystem _healthSystem;


    public override void ResetFeedback()
    {
    }

    public override void StartFeedback()
    {

    }
}
