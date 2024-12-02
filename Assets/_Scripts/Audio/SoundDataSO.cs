using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Sound Data")]
public class SoundDataSO : ScriptableObject, ISound
{
    [SerializeField] private AudioClipsGroup _audioClipGroup = default;
    [SerializeField] private SoundGameEvent _audioChannelEvent;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _playOnAwake;
    [SerializeField][Range(0f, 1f)] private float _volume = 1f;
    [SerializeField] private bool _randomizePitch = false;

    public AudioClip GetClip()
    {
        return _audioClipGroup.GetNextClip();
    }

    public SoundGameEvent AudioChannelEvent => _audioChannelEvent;
    public bool Loop => _loop;
    public bool PlayOnAwake => _playOnAwake;
    public float Volume => _volume;
    public bool RandomizePitch => _randomizePitch;

    public void PlayEvent()
    {
        _audioChannelEvent.RaiseEvent(this);
    }
}

/// <summary>
/// Represents a group of AudioClips that can be treated as one, and provides automatic randomisation or sequencing based on the <c>SequenceMode</c> value.
/// </summary>
[System.Serializable]
public class AudioClipsGroup
{
    public AudioSequenceModeType sequenceMode = AudioSequenceModeType.RandomNoImmediateRepeat;
    public AudioClip[] audioClips;

    private int _nextClipToPlay = -1;
    private int _lastClipPlayed = -1;

    /// <summary>
    /// Chooses the next clip in the sequence, either following the order or randomly.
    /// </summary>
    /// <returns>A reference to an AudioClip</returns>
    public AudioClip GetNextClip()
    {
        // Fast out if there is only one clip to play
        if (audioClips.Length == 1)
            return audioClips[0];

        if (_nextClipToPlay == -1)
        {
            // Index needs to be initialised: 0 if Sequential, random if otherwise
            _nextClipToPlay = (sequenceMode == AudioSequenceModeType.Sequential) ? 0 : Random.Range(0, audioClips.Length);
        }
        else
        {
            // Select next clip index based on the appropriate SequenceMode
            switch (sequenceMode)
            {
                case AudioSequenceModeType.Random:
                    _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    break;

                case AudioSequenceModeType.RandomNoImmediateRepeat:
                    do
                    {
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                    } while (_nextClipToPlay == _lastClipPlayed);
                    break;

                case AudioSequenceModeType.Sequential:
                    _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                    break;
            }
        }

        _lastClipPlayed = _nextClipToPlay;

        return audioClips[_nextClipToPlay];
    }
}