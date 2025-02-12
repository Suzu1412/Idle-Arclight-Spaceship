using System;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private VoidGameEventBinding OnStartGameBinding;
    [SerializeField] private SoundDataSO _playMusic;
    private Action PlayMusicAction;

    private void Awake()
    {
        PlayMusicAction = PlayMusic;
    }

    private void OnEnable()
    {
        OnStartGameBinding.Bind(PlayMusicAction, this);
    }

    private void OnDisable()
    {
        OnStartGameBinding.Unbind(PlayMusicAction, this);
    }

    private void PlayMusic()
    {
        _playMusic.PlayEvent();
    }
}
