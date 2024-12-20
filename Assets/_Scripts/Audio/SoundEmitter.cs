using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using DG.Tweening;

public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;
    private ObjectPooler _pool;

    [Header("Coroutine Variables")]
    private Coroutine _playingSoundCoroutine;
    private Coroutine _playingMusicCoroutine;
    private Coroutine _fadeInMusicCoroutine;
    private Coroutine _fadeOutMusicCoroutine;
    private float _fadeDuration = 2f;
    private bool _isFadingOut = false;
    private ISound _data;
    private AudioClip _currentClip;

    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;
    public event UnityAction<ISound> OnMusicFinishedPlaying;

    public ObjectPooler Pool => _pool = _pool != null ? _pool : _pool = gameObject.GetOrAdd<ObjectPooler>();
    internal AudioSource AudioSource => _audioSource != null ? _audioSource : _audioSource = gameObject.GetOrAdd<AudioSource>();

    private void ApplySettings(ISound data, AudioMixerGroup audioMixer)
    {
        _data = data;
        _currentClip = data.GetClip();
        AudioSource.clip = _currentClip;
        AudioSource.outputAudioMixerGroup = audioMixer;
        AudioSource.volume = data.Volume;
        AudioSource.loop = data.Loop;
        AudioSource.time = 0;
        AudioSource.pitch = 1f;
        AudioSource.playOnAwake = data.PlayOnAwake;

        if (data.RandomizePitch)
        {
            AudioSource.pitch += Random.Range(-0.05f, 0.05f);
        }

    }

    /// <summary>
    /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="settings"></param>
    /// <param name="hasToLoop"></param>
    /// <param name="position"></param>
    public void PlaySoundClip(ISound sound, AudioMixerGroup audioMixer, Vector3 position = default)
    {
        ApplySettings(sound, audioMixer);
        AudioSource.transform.position = position;
        AudioSource.Play();

        if (!sound.Loop)
        {
            if (_playingSoundCoroutine != null) StopCoroutine(_playingSoundCoroutine);
            _playingSoundCoroutine = StartCoroutine(SoundFinishedPlaying(_currentClip.length));
        }
    }

    public void PlayMusicClip(ISound sound, AudioMixerGroup audioMixer)
    {
        ApplySettings(sound, audioMixer);
        AudioSource.volume = 0f;
        AudioSource.Play();
        AudioSource.DOFade(_data.Volume, 1f);


        _playingMusicCoroutine = StartCoroutine(MusicFinishedPlaying(sound, _currentClip.length - _fadeDuration - 3f));
    }

    /// <summary>
	/// Used when the game is unpaused, to pick up SFX from where they left.
	/// </summary>
	public void Resume()
    {
        AudioSource.Play();
    }

    /// <summary>
    /// Used when the game is paused.
    /// </summary>
    public void Pause()
    {
        AudioSource.Pause();
    }

    /// <summary>
    /// Used when the SFX finished playing. Called by the <c>AudioManager</c>.
    /// </summary>
    public void Stop()
    {
        AudioSource.Stop();
    }

    public void Finish()
    {
        if (_audioSource.loop)
        {
            _audioSource.loop = false;
            if (_playingSoundCoroutine != null)
            {
                StopCoroutine(_playingSoundCoroutine);
                _playingSoundCoroutine = null;
            }
        }

        float timeRemaining = _audioSource.clip.length - _audioSource.time;
        _playingSoundCoroutine = StartCoroutine(SoundFinishedPlaying(timeRemaining));
    }

    public bool IsPlaying()
    {
        return AudioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return AudioSource.loop;
    }

    public bool IsFinishing()
    {
        return !AudioSource.loop;
    }

    /// <summary>
	/// Used to check which music track is being played.
	/// </summary>
	public AudioClip GetClip()
    {
        return AudioSource.clip;
    }

    IEnumerator SoundFinishedPlaying(float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }

    IEnumerator MusicFinishedPlaying(ISound sound, float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        AudioSource.DOFade(0f, 1f);
        yield return Helpers.GetWaitForSeconds(1f);
        OnMusicFinishedPlaying?.Invoke(sound);
    }
}
