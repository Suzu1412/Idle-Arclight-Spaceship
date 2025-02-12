using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private FloatVariableSO _masterVolume;
    [SerializeField] private FloatVariableSO _musicVolume;
    [SerializeField] private FloatVariableSO _sfxVolume;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
    [SerializeField] private FloatGameEvent OnMasterVolumeChangedEvent = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField] private FloatGameEvent OnMusicVolumeChangedEvent = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField] private FloatGameEvent OnSFXVolumeChangedEvent = default;


    private void OnEnable()
    {
        _sfxSlider.value = _sfxVolume.Value;
        _musicSlider.value = _musicVolume.Value;
    }

    public void MusicVolumeChanged()
    {
        _musicVolume.Initialize(_musicSlider.value, 0f, 1f);
        OnMusicVolumeChangedEvent.RaiseEvent(_musicVolume.Value, this);
    }

    public void SFXVolumeChanged()
    {
        _sfxVolume.Initialize(_sfxSlider.value, 0f, 1f);
        OnSFXVolumeChangedEvent.RaiseEvent(_sfxVolume.Value, this);
    }
}
