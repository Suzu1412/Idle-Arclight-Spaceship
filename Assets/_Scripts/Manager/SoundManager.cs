using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("SoundEmitters pool")]
    [SerializeField] private ObjectPoolSettingsSO _soundPool;
    [SerializeField] private readonly List<SoundEmitter> _activeSoundEmitters = new();

    // Ensure that there won't be too many sounds playing at the same time
    public readonly Dictionary<SoundDataSO, int> Counts = new();

    [SerializeField] private int _maxSoundInstances = 30;

    [Header("Event Listener")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField] private SoundGameEventListener _sfxEventChannel = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
    [SerializeField] private SoundGameEventListener _musicEventChannel = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField] private FloatGameEventListener _SFXVolumeChangeEventChannel = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField] private FloatGameEventListener _musicVolumeChangeEventChannel = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
    [SerializeField] private FloatGameEventListener _masterVolumeChangeEventChannel = default;

    [Header("Audio Mixer Group")]
    [SerializeField] private AudioMixerGroup _masterMixerGroup = default;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup = default;
    [SerializeField] private AudioMixerGroup _musicMixerGroup = default;

    private void OnEnable()
    {
        _sfxEventChannel.Register(PlaySound);
        //_musicEventChannel.Register(PlayMusicTrack);

        //_masterVolumeChangeEventChannel.Register(ChangeMasterVolume);
        //_musicVolumeChangeEventChannel.Register(ChangeMusicVolume);
        //_SFXVolumeChangeEventChannel.Register(ChangeSFXVolume);
    }

    private void OnDisable()
    {
        _sfxEventChannel.DeRegister(PlaySound);
        //_musicEventChannel.DeRegister(PlayMusicTrack);

        //_masterVolumeChangeEventChannel.DeRegister(ChangeMasterVolume);
        //_musicVolumeChangeEventChannel.DeRegister(ChangeMusicVolume);
        //_SFXVolumeChangeEventChannel.DeRegister(ChangeSFXVolume);
    }

    private bool CanPlaySound(SoundDataSO data)
    {
        return !Counts.TryGetValue(data, out var count) || count < _maxSoundInstances;
    }

    private void PlaySound(ISound sound)
    {
        SoundEmitter soundEmitter = ObjectPoolFactory.Spawn(_soundPool).GetComponent<SoundEmitter>();
        soundEmitter.Initialize(sound, _sfxMixerGroup);
        //_sfxSoundEmitterList.Add(soundEmitter);

        soundEmitter.PlayAudioClip(sound);

        if (!sound.Loop)
        {
            soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
        soundEmitter.Stop();
        //_sfxSoundEmitterList.Remove(soundEmitter);
        ObjectPoolFactory.ReturnToPool(soundEmitter.Pool);
    }
}
