using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private VoidGameEventListener OnGameStartListener;
    [SerializeField] private SoundDataSO _playMusic;

    private void OnEnable()
    {
        OnGameStartListener.Register(PlayMusic);
    }

    private void OnDisable()
    {
        OnGameStartListener.DeRegister(PlayMusic);
    }

    private void PlayMusic()
    {
        _playMusic.PlayEvent();
    }
}
