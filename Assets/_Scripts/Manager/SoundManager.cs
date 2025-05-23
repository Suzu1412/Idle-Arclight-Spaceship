using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;

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

    private UnityAction<SoundEmitter> soundEmitterFinished;

    private void Awake()
    {
        soundEmitterFinished = OnSoundEmitterFinishedPlaying;
    }

    private void OnEnable()
    {
        _saveable.Add(this);
        OnPlaySfxEventListener.Register(PlaySound);
        OnPlayMusicEventListener.Register(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.Register(SetMasterVolume);
        OnMusicVolumeChangedEventListener.Register(SetMusicVolume);
        OnSFXVolumeChangedEventListener.Register(SetSFXVolume);
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnPlaySfxEventListener.DeRegister(PlaySound);
        OnPlayMusicEventListener.DeRegister(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.DeRegister(SetMasterVolume);
        OnMusicVolumeChangedEventListener.DeRegister(SetMusicVolume);
        OnSFXVolumeChangedEventListener.DeRegister(SetSFXVolume);
    }

    public void SaveData(GameDataSO gameData)
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume.Value);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume.Value);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume.Value);
    }

    public void LoadData(GameDataSO gameData)
    {
        float masterVolume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : 1f;
        float musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 1f;
        float sfxVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 1f;

        _masterVolume.Initialize(masterVolume, 0f, 1f);
        _musicVolume.Initialize(musicVolume, 0f, 1f);
        _sfxVolume.Initialize(sfxVolume, 0f, 1f);

        SetMasterVolume(_masterVolume.Value);
        SetMusicVolume(_musicVolume.Value);
        SetSFXVolume(_sfxVolume.Value);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
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
            soundEmitter.OnSoundFinishedPlaying += soundEmitterFinished;
        }
    }

    private void PlayMusicTrack(ISound sound)
    {
        if (_activeMusicEmitter == null)
        {
            _activeMusicEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        }

        _activeMusicEmitter.PlayMusicClip(sound, _musicMixerGroup);

        if (!sound.Loop) // playlist functionality depends on not active
        {
            _activeMusicEmitter.OnMusicFinishedPlaying += OnMusicEmitterFinishedPlaying;
        }
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= soundEmitterFinished;
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
        return Mathf.Log10(1 + 9 * mixerValue) / Mathf.Log10(10);
        //return 1f + (mixerValue / 80f);
    }
    private float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return Mathf.Log10(Mathf.Max(normalizedValue, 0.0001f)) * 20;
        //return (normalizedValue - 1f) * 80f;
    }

}
