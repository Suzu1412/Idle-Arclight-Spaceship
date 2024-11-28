using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Sound Data")]
public class SoundDataSO : ScriptableObject, ISound
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private SoundGameEvent _audioChannelEvent;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _playOnAwake;
    [SerializeField][Range(0f, 1f)] private float _volume = 1f;
    [SerializeField] private bool _randomizePitch = false;

    public AudioClip Clip => _clip;
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
