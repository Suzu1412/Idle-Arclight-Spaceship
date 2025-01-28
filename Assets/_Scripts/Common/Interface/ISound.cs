using UnityEngine;

public interface ISound
{
    bool Loop { get; }
    bool RandomizePitch { get; }
    float Volume { get; }
    bool PlayOnAwake { get; }

    AudioClip GetClip();

    void PlayEvent();
}
