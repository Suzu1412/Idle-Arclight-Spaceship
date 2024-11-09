using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCamera;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroup[] _sceneGroups;

    [Header("Bool Event")]
    [SerializeField] private BoolGameEvent OnSceneGroupLoadedEvent;
    [SerializeField] private BoolGameEvent OnToggleLoadEvent;

    [Header("Float Event")]
    [SerializeField] private FloatGameEvent OnLoadProgressEvent;

    [Header("Bool Event Listener")]
    [SerializeField] private BoolGameEventListener OnToggleLoadEventListener;

    [SerializeField] private float _fillSpeed = 0.5f;
    private float _targetProgress;
    private float _currentFillAmount;
    private float _loadedScenes;
    private bool _isLoading;

    public readonly SceneGroupManager _manager = new();

    private void Awake()
    {
        _manager.OnSceneLoaded += sceneName => SceneLoaded(sceneName);
        _manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded" + sceneName);
        _manager.OnSceneGroupLoaded += () => SceneGroupLoaded();
        _manager.OnSceneGroupUnloaded += () => UpdateLoadProgress(0.2f);
    }

    async void Start()
    {
        await LoadSceneGroup(0);
    }

    private void OnEnable()
    {
        OnToggleLoadEventListener.Register(EnableLoadingCanvas);
    }

    private void OnDisable()
    {
        OnToggleLoadEventListener.DeRegister(EnableLoadingCanvas);
    }

    public async Awaitable LoadSceneGroup(int index)
    {
        _targetProgress = 1f;
        _loadedScenes = 0;

        if (index < 0 || index >= _sceneGroups.Length)
        {
            Debug.LogError("Invalid Scene group index: " + index);
            index = 0;
        }

        UpdateLoadProgress(0f);
        OnToggleLoadEvent.RaiseEvent(true);
        await _manager.LoadScenes(_sceneGroups[index]);
        //EnableLoadingCanvas(false);
    }

    void EnableLoadingCanvas(bool enable = true)
    {
        _isLoading = enable;
        _loadingCanvas.gameObject.SetActive(enable);
        _loadingCamera.SetActive(enable);

        if (enable)
        {
            _currentFillAmount = 0f;
        }
    }

    private void SceneLoaded(string sceneName)
    {
        _loadedScenes++;
        float progress = 0.2f + _loadedScenes * ((1f - 0.2f) / _sceneGroups[0].Scenes.Count);
        Debug.Log(progress);
        UpdateLoadProgress(progress);
    }

    private void SceneGroupLoaded()
    {
        OnSceneGroupLoadedEvent.RaiseEvent(true);
    }

    private void UpdateLoadProgress(float value)
    {
        OnLoadProgressEvent.RaiseEvent(value);
    }
}
