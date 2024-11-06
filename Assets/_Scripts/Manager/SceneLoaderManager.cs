using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] private Camera _loadingCamera;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroup[] _sceneGroups;

    [Header("Bool Event")]
    [SerializeField] private BoolGameEvent OnSceneGroupLoadedEvent;

    [Header("Float Event")]
    [SerializeField] private FloatGameEvent OnLoadProgressEvent;

    [SerializeField] private float _fillSpeed = 0.5f;
    private float _targetProgress;
    private float _currentFillAmount;
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

        float progressDifference = Mathf.Abs(_currentFillAmount - _targetProgress);
        float dynamicFillSpeed = progressDifference * _fillSpeed;

        _currentFillAmount = Mathf.Lerp(_currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
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

        Debug.Log(_targetProgress);

        EnableLoadingCanvas();
        await _manager.LoadScenes(_sceneGroups[index], progress);
        EnableLoadingCanvas(false);

    }

    void EnableLoadingCanvas(bool enable = true)
    {
        _isLoading = enable;
        _loadingCanvas.gameObject.SetActive(enable);
        _loadingCamera.gameObject.SetActive(enable);

        if (enable)
        {
            _currentFillAmount = 0f;
        }
        // loading canvas set active true
        // loading camera set active true
    }
}
