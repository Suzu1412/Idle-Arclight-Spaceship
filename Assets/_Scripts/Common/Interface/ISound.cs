using UnityEngine;

public interface ISound
{
    AudioClip Clip { get; }
    SoundGameEvent AudioChannelEvent { get; }
    bool Loop { get; }
    bool RandomizePitch { get; }
    float Volume { get; }
    bool PlayOnAwake { get; }

    void PlayEvent();
}
