using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class SoundManager : MonoBehaviour, ISaveable
{
    [Header("SoundEmitters pool")]
    [SerializeField] private ObjectPoolSettingsSO _soundPool;
    [SerializeField][ReadOnly] private SoundEmitter _activeMusicEmitter;
    [SerializeField] private readonly List<SoundEmitter> _activeSoundEmitters = new();

    // Ensure that there won't be too many sounds playing at the same time
    public readonly Dictionary<SoundDataSO, int> Counts = new();

    [SerializeField] private int _maxSoundInstances = 30;

    [Header("Event Listener")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField] private SoundGameEventListener OnPlaySfxEventListener = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
    [SerializeField] private SoundGameEventListener OnPlayMusicEventListener = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
    [SerializeField] private FloatGameEventListener OnMasterVolumeChangedEventListener = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField] private FloatGameEventListener OnMusicVolumeChangedEventListener = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField] private FloatGameEventListener OnSFXVolumeChangedEventListener = default;

    [Header("Audio Mixer Group")]
    [SerializeField] private AudioMixer _audioMixer = default;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup = default;
    [SerializeField] private AudioMixerGroup _musicMixerGroup = default;

    [SerializeField] private FloatVariableSO _masterVolume;
    [SerializeField] private FloatVariableSO _musicVolume;
    [SerializeField] private FloatVariableSO _sfxVolume;

    private void OnEnable()
    {
        OnPlaySfxEventListener.Register(PlaySound);
        OnPlayMusicEventListener.Register(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.Register(SetMasterVolume);
        OnMusicVolumeChangedEventListener.Register(SetMusicVolume);
        OnSFXVolumeChangedEventListener.Register(SetSFXVolume);
    }

    private void OnDisable()
    {
        OnPlaySfxEventListener.DeRegister(PlaySound);
        OnPlayMusicEventListener.DeRegister(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.DeRegister(SetMasterVolume);
        OnMusicVolumeChangedEventListener.DeRegister(SetMusicVolume);
        OnSFXVolumeChangedEventListener.DeRegister(SetSFXVolume);
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.SaveVolume(_masterVolume.Value, _musicVolume.Value, _sfxVolume.Value);
    }

    public void LoadData(GameDataSO gameData)
    {
        _masterVolume.Initialize(gameData.VolumeData.MasterVolume, 0f, 1f);
        _musicVolume.Initialize(gameData.VolumeData.MusicVolume, 0f, 1f);
        _sfxVolume.Initialize(gameData.VolumeData.SFXVolume, 0f, 1f);

        SetMasterVolume(_masterVolume.Value);
        SetMusicVolume(_musicVolume.Value);
        SetSFXVolume(_sfxVolume.Value);
    }

    private bool CanPlaySound(SoundDataSO data)
    {
        return !Counts.TryGetValue(data, out var count) || count < _maxSoundInstances;
    }

    private void PlaySound(ISound sound)
    {
        if (!CanPlaySound(sound as SoundDataSO))
        {
            return;
        }

        SoundEmitter soundEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        if (soundEmitter.IsPlaying())
        {
            soundEmitter.Finish();
        }

        _activeSoundEmitters.Add(soundEmitter);

        soundEmitter.PlaySoundClip(sound, _sfxMixerGroup);

        if (!sound.Loop)
        {
            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }
    }

    private void PlayMusicTrack(ISound sound)
    {
        float fadeDuration = 2f;
        float startTime = 0f;

        if (_activeMusicEmitter == null)
        {
            _activeMusicEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        }

        if (_activeMusicEmitter != null && _activeMusicEmitter.IsPlaying())
        {
            startTime = _activeMusicEmitter.FadeMusicOut(fadeDuration);

        }

        _activeMusicEmitter.FadeMusicIn(sound, _musicMixerGroup, 1f, startTime);

        if (!sound.Loop) // playlist functionality depends on not active
        {
            _activeMusicEmitter.OnMusicFinishedPlaying += OnMusicEmitterFinishedPlaying;
        }
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        soundEmitter.Stop();
        _activeSoundEmitters.Remove(soundEmitter);
        ObjectPoolFactory.ReturnToPool(soundEmitter.Pool);
    }

    private void OnMusicEmitterFinishedPlaying(ISound sound)
    {
        PlayMusicTrack(sound);
        _activeMusicEmitter.OnMusicFinishedPlaying -= OnMusicEmitterFinishedPlaying;
    }

    private void SetMasterVolume(float volume)
    {
        SetGroupVolume("MasterVolume", volume);
    }

    private void SetMusicVolume(float volume)
    {
        SetGroupVolume("MusicVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        SetGroupVolume("SFXVolume", volume);
    }

    private float GetMasterVolume()
    {
        return GetGroupVolume("MasterVolume");
    }

    private float GetMusicVolume()
    {
        return GetGroupVolume("MusicVolume");
    }

    private float GetSFXVolume()
    {
        return GetGroupVolume("SFXVolume");
    }

    // Audio Mixer
    public float GetGroupVolume(string parameterName)
    {
        if (_audioMixer.GetFloat(parameterName, out float rawVolume))
        {
            return MixerValueToNormalized(rawVolume);
        }
        else
        {
            Debug.LogError("The AudioMixer parameter was not found");
            return 0f;
        }
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = _audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
            Debug.LogError("The AudioMixer parameter was not found");
    }

    // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
    /// when using UI sliders normalized format
    private float MixerValueToNormalized(float mixerValue)
    {
        // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
        return 1f + (mixerValue / 80f);
    }
    private float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return (normalizedValue - 1f) * 80f;
    }

}
