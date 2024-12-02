using UnityEngine;

public interface ISound
{
    SoundGameEvent AudioChannelEvent { get; }
    bool Loop { get; }
    bool RandomizePitch { get; }
    float Volume { get; }
    bool PlayOnAwake { get; }

    AudioClip GetClip();

    void PlayEvent();
}
