using UnityEngine;

public class UISoundEffect : MonoBehaviour
{
    [SerializeField] private SoundDataSO _playSFX;

    public void PlaySFX()
    {
        _playSFX.PlayEvent();
    }
}
