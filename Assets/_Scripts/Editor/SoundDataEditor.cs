using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundDataSO))]
public class SoundDataEditor : Editor
{
    private AudioSource _previewSource;
    private bool _isPlaying;
    private bool _loopPreview;
    private float _previewVolume = 1f;

    private void OnEnable()
    {
        // Create a hidden GameObject for audio previewing
        if (_previewSource == null)
        {
            GameObject previewObject = EditorUtility.CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave, typeof(AudioSource));
            _previewSource = previewObject.GetComponent<AudioSource>();
        }
    }

    private void OnDisable()
    {
        StopSound();
        if (_previewSource != null)
        {
            DestroyImmediate(_previewSource.gameObject);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws default fields

        SoundDataSO soundData = (SoundDataSO)target;
        if (soundData == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🎵 Sound Preview", EditorStyles.boldLabel);

        AudioClip clip = soundData.GetClip();
        if (clip != null)
        {
            EditorGUILayout.LabelField("Next Clip:", clip.name);
        }
        else
        {
            EditorGUILayout.HelpBox("No AudioClips available!", MessageType.Warning);
        }

        // Volume control
        _previewVolume = EditorGUILayout.Slider("Volume", _previewVolume, 0f, 1f);

        // Loop toggle
        _loopPreview = EditorGUILayout.Toggle("Loop Sound", _loopPreview);

        // Play, Pause, Stop buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("▶ Play Next"))
        {
            PlaySound(soundData);
        }
        if (GUILayout.Button("⏸ Pause"))
        {
            PauseSound();
        }
        if (GUILayout.Button("⏹ Stop"))
        {
            StopSound();
        }
        EditorGUILayout.EndHorizontal();

        // Handle spacebar shortcut for playing
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        {
            PlaySound(soundData);
            Event.current.Use();
        }
    }

    private void PlaySound(SoundDataSO soundData)
    {
        if (_previewSource == null) return;

        AudioClip clip = soundData.GetClip(); // Gets the next clip based on sequence mode
        if (clip == null) return;

        _previewSource.clip = clip;
        _previewSource.volume = _previewVolume;
        _previewSource.loop = _loopPreview;

        if (soundData.RandomizePitch)
        {
            _previewSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else
        {
            _previewSource.pitch = 1f;
        }

        _previewSource.Play();
        _isPlaying = true;
    }

    private void PauseSound()
    {
        if (_previewSource != null && _isPlaying)
        {
            _previewSource.Pause();
            _isPlaying = false;
        }
    }

    private void StopSound()
    {
        if (_previewSource != null)
        {
            _previewSource.Stop();
            _isPlaying = false;
        }
    }
}
