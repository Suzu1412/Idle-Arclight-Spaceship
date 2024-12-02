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
    public ObjectPooler Pool => _pool = _pool != null ? _pool : _pool = gameObject.GetOrAdd<ObjectPooler>();
    internal AudioSource AudioSource => _audioSource != null ? _audioSource : _audioSource = gameObject.GetOrAdd<AudioSource>();

    public void Initialize(ISound data, AudioMixerGroup audioMixer)
    {
        _data = data;
        AudioSource.outputAudioMixerGroup = audioMixer;
        AudioSource.volume = data.Volume;
        AudioSource.loop = data.Loop;
        AudioSource.playOnAwake = data.PlayOnAwake;

        if (data.RandomizePitch)
        {
            _audioSource.pitch = 1f;
            _audioSource.pitch += Random.Range(-3f, 3f);
        }

    }

    /// <summary>
    /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="settings"></param>
    /// <param name="hasToLoop"></param>
    /// <param name="position"></param>
    public void PlaySoundClip(ISound sound, Vector3 position = default)
    {
        _currentClip = sound.GetClip();
        _audioSource.clip = _currentClip;
        _audioSource.transform.position = position;
        _audioSource.loop = sound.Loop;
        _audioSource.Play();

        if (!sound.Loop)
        {
            _playingSoundCoroutine = StartCoroutine(SoundFinishedPlaying(_currentClip.length));
        }
    }

    public void PlayMusicClip()
    {
        FadeMusicOut(2f);
        FadeMusicIn(2f, 0f);

        _playingMusicCoroutine = StartCoroutine(MusicFinishedPlaying(_currentClip.length));
    }

    /// <summary>
	/// Used when the game is unpaused, to pick up SFX from where they left.
	/// </summary>
	public void Resume()
    {
        _audioSource.Play();
    }

    /// <summary>
    /// Used when the game is paused.
    /// </summary>
    public void Pause()
    {
        _audioSource.Pause();
    }

    /// <summary>
    /// Used when the SFX finished playing. Called by the <c>AudioManager</c>.
    /// </summary>
    public void Stop()
    {
        if (_playingSoundCoroutine != null)
        {
            StopCoroutine(_playingSoundCoroutine);
            _playingSoundCoroutine = null;
        }

        _audioSource.Stop();
        OnSoundFinishedPlaying?.Invoke(this);
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

    public bool IsInUse()
    {
        return _audioSource.isPlaying;
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return _audioSource.loop;
    }

    public bool IsFinishing()
    {
        return !_audioSource.loop;
    }

    /// <summary>
	/// Used to check which music track is being played.
	/// </summary>
	public AudioClip GetClip()
    {
        return _audioSource.clip;
    }

    IEnumerator SoundFinishedPlaying(float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }

    IEnumerator MusicFinishedPlaying(float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        PlayMusicClip();
    }

    internal void FadeMusicIn(float duration, float startTime = 0f)
    {
        PlayMusicClip();
        _audioSource.volume = 0f;

        //Start the clip at the same time the previous one left, if length allows
        //TODO: find a better way to sync fading songs
        if (startTime <= _audioSource.clip.length)
            _audioSource.time = startTime;

        _audioSource.DOFade(_data.Volume, duration);
    }

    internal float FadeMusicOut(float duration)
    {
        _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

        return _audioSource.time;
    }

    private void OnFadeOutComplete()
    {
        OnSoundFinishedPlaying.Invoke(this);
    }

}
