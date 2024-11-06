using System;
using UnityEngine;
using UnityEngine.Events;

public class LoadingProgress : IProgress<float>
{
    public event UnityAction<float> Progressed;

    const float ratio = 1f;

    public void Report(float value)
    {
        Progressed?.Invoke(value / ratio);
    }
}
