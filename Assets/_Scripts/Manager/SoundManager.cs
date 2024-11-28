using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class SoundManager : MonoBehaviour, ISaveable
{
    [Header("SoundEmitters pool")]
    [SerializeField] private ObjectPoolSettingsSO _soundPool;
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
    [SerializeField] private AudioMixerGroup _masterMixerGroup = default;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup = default;
    [SerializeField] private AudioMixerGroup _musicMixerGroup = default;

    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;

    private void OnEnable()
    {
        OnPlaySfxEventListener.Register(PlaySound);
        OnPlayMusicEventListener.Register(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.Register(ChangeMasterVolume);
        OnMusicVolumeChangedEventListener.Register(ChangeMusicVolume);
        OnSFXVolumeChangedEventListener.Register(ChangeSFXVolume);
    }

    private void OnDisable()
    {
        OnPlaySfxEventListener.DeRegister(PlaySound);
        OnPlayMusicEventListener.DeRegister(PlayMusicTrack);

        OnMasterVolumeChangedEventListener.DeRegister(ChangeMasterVolume);
        OnMusicVolumeChangedEventListener.DeRegister(ChangeMusicVolume);
        OnSFXVolumeChangedEventListener.DeRegister(ChangeSFXVolume);
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.SaveVolume(_masterVolume, _musicVolume, _sfxVolume);
    }

    public void LoadData(GameDataSO gameData)
    {
        ChangeMasterVolume(gameData.VolumeData.MasterVolume);
        ChangeMusicVolume(gameData.VolumeData.MusicVolume);
        ChangeSFXVolume(gameData.VolumeData.SFXVolume);

    }

    private bool CanPlaySound(SoundDataSO data)
    {
        return !Counts.TryGetValue(data, out var count) || count < _maxSoundInstances;
    }

    private void PlaySound(ISound sound)
    {
        SoundEmitter soundEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        soundEmitter.Initialize(sound, _sfxMixerGroup);
        _activeSoundEmitters.Add(soundEmitter);

        soundEmitter.PlayAudioClip(sound);

        if (!sound.Loop)
        {
            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }
    }

    private void PlayMusicTrack(ISound music)
    {
        SoundEmitter soundEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        soundEmitter.Initialize(music, _musicMixerGroup);
        _activeSoundEmitters.Add(soundEmitter);

        soundEmitter.PlayAudioClip(music);

        if (!music.Loop)
        {
            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        soundEmitter.Stop();
        _activeSoundEmitters.Remove(soundEmitter);
        ObjectPoolFactory.ReturnToPool(soundEmitter.Pool);
    }

    private void ChangeMasterVolume(float volume)
    {
        _masterVolume = volume;
        _masterMixerGroup.audioMixer.SetFloat("MasterVolume", volume);
    }

    private void ChangeMusicVolume(float volume)
    {
        _musicVolume = volume;
        _musicMixerGroup.audioMixer.SetFloat("MusicVolume", volume);
    }

    private void ChangeSFXVolume(float volume)
    {
        _sfxVolume = volume;
        _sfxMixerGroup.audioMixer.SetFloat("SFXVolume", volume);
    }

}
