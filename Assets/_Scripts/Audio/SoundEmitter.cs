using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;
    private ObjectPooler _pool;

    [Header("Coroutine Variables")]
    private Coroutine _playingCoroutine;
    private Coroutine _fadeInMusicCoroutine;
    private Coroutine _fadeOutMusicCoroutine;
    private float _fadeDuration = 2f;
    private bool _isFadingOut = false;

    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : _pool = gameObject.GetOrAdd<ObjectPooler>();
    internal AudioSource AudioSource => _audioSource != null ? _audioSource : _audioSource = gameObject.GetOrAdd<AudioSource>();

    public void Initialize(ISound data, AudioMixerGroup audioMixer)
    {
        AudioSource.clip = data.Clip;
        AudioSource.outputAudioMixerGroup = audioMixer;
        AudioSource.volume = data.Volume;
        AudioSource.loop = data.Loop;
        AudioSource.playOnAwake = data.PlayOnAwake;

        if (data.RandomizePitch)
        {
            _audioSource.pitch = 1f;
            _audioSource.pitch += Random.Range(-0.05f, 0.05f);
        }

    }

    public void Play(SoundDataSO data)
    {
        if (_playingCoroutine != null) { StopCoroutine(_playingCoroutine); }

        AudioSource.Play();
        _playingCoroutine = StartCoroutine(FinishedPlaying(data.Clip.length));
    }

    /// <summary>
    /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="settings"></param>
    /// <param name="hasToLoop"></param>
    /// <param name="position"></param>
    public void PlayAudioClip(ISound sound, Vector3 position = default)
    {
        _audioSource.clip = sound.Clip;
        _audioSource.transform.position = position;
        _audioSource.loop = sound.Loop;
        _audioSource.Play();

        if (sound.RandomizePitch)
        {
            _audioSource.pitch = 1f;
            _audioSource.pitch += Random.Range(-0.05f, 0.05f);
        }

        if (!sound.Loop)
        {
            StartCoroutine(FinishedPlaying(sound.Clip.length));
        }
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
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }

        _audioSource.Stop();
        OnSoundFinishedPlaying?.Invoke(this);
    }

    public void Finish()
    {
        if (_audioSource.loop)
        {
            _audioSource.loop = false;
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }
        }

        float timeRemaining = _audioSource.clip.length - _audioSource.time;
        _playingCoroutine = StartCoroutine(FinishedPlaying(timeRemaining));
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

    IEnumerator FinishedPlaying(float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }

    internal void FadeMusicIn(float start, float end)
    {
        if (_fadeInMusicCoroutine != null) StopCoroutine(_fadeInMusicCoroutine);
        _fadeInMusicCoroutine = StartCoroutine(FadeMusicInCoroutine(start, end));
    }

    internal void FadeMusicOut()
    {

    }

    IEnumerator FadeMusicInCoroutine(float start, float end)
    {
        float timeElapsed = 0f;

        while (timeElapsed < _fadeDuration)
        {
            if (!_isFadingOut)
            {
                float t = timeElapsed / _fadeDuration;
                _audioSource.volume = Mathf.Lerp(start, end, t);
            }

            yield return null;
        }

        _audioSource.volume = end;
    }

    IEnumerator FadeMusicOutCoroutine()
    {
        float timeElapsed = 0f;
        float start = _audioSource.volume;
        _isFadingOut = true;

        while (timeElapsed < _fadeDuration)
        {
            float t = timeElapsed / _fadeDuration;
            _audioSource.volume = Mathf.Lerp(start, 0f, t);

            yield return null;
        }

        _audioSource.volume = 0f;
        _isFadingOut = false;
    }

}
