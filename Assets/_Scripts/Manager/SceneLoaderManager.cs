using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour, ISaveable
{
    private int _currentScene;
    [SerializeField] private GameObject _loadingCamera;
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private SceneGroup[] _sceneGroups;

    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnLoadLastSceneEvent;
    [SerializeField] private VoidGameEvent OnFinishedLoadingEvent;

    [Header("Int Event")]
    [SerializeField] private IntGameEventListener OnChangeSceneEventListener;

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

    private void Start()
    {
        OnLoadLastSceneEvent.RaiseEvent();
    }

    private void OnEnable()
    {
        OnToggleLoadEventListener.Register(EnableLoadingCanvas);
        OnChangeSceneEventListener.Register(ChangeScene);
    }

    private void OnDisable()
    {
        OnToggleLoadEventListener.DeRegister(EnableLoadingCanvas);
        OnChangeSceneEventListener.DeRegister(ChangeScene);
    }

    private async void ChangeScene(int level)
    {
        await LoadSceneGroup(level);
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
        EnableLoadingCanvas(false);
        OnFinishedLoadingEvent.RaiseEvent();
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

    public void SaveData(GameDataSO gameData)
    {
        gameData.SaveCurrentScene(_currentScene);
    }

    public void LoadData(GameDataSO gameData)
    {
        // Loaded Scene Data must be done before
    }
}
