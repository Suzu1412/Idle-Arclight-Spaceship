using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private float _unloadProgress = 0.2f;
    private float _loadedScenes;

    public readonly SceneGroupManager _manager = new();

    private void Awake()
    {
        _manager.OnSceneLoaded += SceneLoaded;
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
        _loadedScenes = 0;

        if (index < 0 || index >= _sceneGroups.Length)
        {
            Debug.LogError("Invalid Scene group index: " + index);
            index = 0;
        }

        UpdateLoadProgress(0f);
        OnToggleLoadEvent.RaiseEvent(true);
        await _manager.LoadScenes(_sceneGroups[index]);
        await Awaitable.WaitForSecondsAsync(0.5f);
        EnableLoadingCanvas(false);
    }

    void EnableLoadingCanvas(bool enable = true)
    {
        _loadingCanvas.gameObject.SetActive(enable);
        _loadingCamera.SetActive(enable);
    }

    private void SceneLoaded(string sceneName)
    {
        _loadedScenes++;
        float progress = _unloadProgress + _loadedScenes * ((1f - _unloadProgress) / _sceneGroups[0].Scenes.Count);
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
