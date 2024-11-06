using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] private Camera _loadingCamera;
    [SerializeField] private SceneGroup[] _sceneGroups;

    [Header("Bool Event")]
    [SerializeField] private BoolGameEvent OnSceneGroupLoadedEvent;

    private float _targetProgress;
    private bool _isLoading;

    public readonly SceneGroupManager _manager = new();

    private void Awake()
    {
        _manager.OnSceneLoaded += sceneName => Debug.Log("Loaded" + sceneName);
        _manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded" + sceneName);
        _manager.OnSceneGroupLoaded += () => OnSceneGroupLoadedEvent.RaiseEvent(true);
    }

    async void Start()
    {
        await LoadSceneGroup(0);
    }

    private void Update()
    {
        if (!_isLoading) return;

        float currentFillAmount = 0;

    }

    public async Awaitable LoadSceneGroup(int index)
    {
        _targetProgress = 1f;

        if (index < 0 || index >= _sceneGroups.Length)
        {
            Debug.LogError("Invalid Scene group index: " + index);
            return;
        }

        LoadingProgress progress = new();

        progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

        EnableLoadingCanvas();
        await _manager.LoadScenes(_sceneGroups[index], progress);
        EnableLoadingCanvas(false);

    }

    void EnableLoadingCanvas(bool enable = true)
    {
        _isLoading = enable;
        _loadingCamera.gameObject.SetActive(enable);
        // loading canvas set active true
        // loading camera set active true
    }
}
