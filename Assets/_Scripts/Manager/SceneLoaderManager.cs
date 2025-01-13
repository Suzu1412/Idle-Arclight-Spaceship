using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : Singleton<MonoBehaviour>, ISaveable
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
    [SerializeField] private FloatGameEvent OnScreenFadeOutEvent;
    [SerializeField] private FloatGameEvent OnScreenFadeInEvent;


    [Header("Bool Event Listener")]
    [SerializeField] private BoolGameEventListener OnToggleLoadEventListener;

    [SerializeField] private SceneReference _sceneLoader;

    [SerializeField] private float _unloadProgress = 0.25f;

    private int _scenesToUnload;
    private int _scenesUnloaded;
    private int _scenesToLoad;
    private int _scenesLoaded;

    // Lists to track the unload operations
    private List<AsyncOperation> _sceneManagerUnloadOperations = new List<AsyncOperation>();
    private List<AsyncOperationHandle<SceneInstance>> _addressableUnloadOperations = new List<AsyncOperationHandle<SceneInstance>>();

    // Lists to track the load operations
    private List<AsyncOperation> _sceneManagerLoadOperations = new List<AsyncOperation>();
    private List<AsyncOperationHandle<SceneInstance>> _addressableLoadOperations = new List<AsyncOperationHandle<SceneInstance>>();

    // Addressable Scene Dictionary
    private Dictionary<string, AsyncOperationHandle<SceneInstance>> _addressableScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

    SceneGroup ActiveSceneGroup;

    private UnityAction OnUnloadScene;
    private UnityAction OnLoadScene;

    private bool _isLoading = false;

    private void Start()
    {
        OnLoadLastSceneEvent.RaiseEvent();
        OnChangeSceneEventListener.Register(ChangeScene);

        OnUnloadScene += UpdateLoadingBarOnUnload;

        OnLoadScene += UpdateLoadingBarOnLoad;
    }

    private void OnDestroy()
    {
        OnChangeSceneEventListener.DeRegister(ChangeScene);
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.SaveCurrentScene(_currentScene);
    }

    public void LoadData(GameDataSO gameData)
    {
        // Loaded Scene Data must be done before
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void ChangeScene(int index)
    {
        _scenesUnloaded = 0;
        _scenesLoaded = 0;

        if (index < 0 || index >= _sceneGroups.Length)
        {
            Debug.LogError("Invalid Scene group index: " + index);
            return;
        }

        if (_sceneGroups[index].Scene == null)
        {
            Debug.LogError($"Scene Index: {index}. Has no Scene assigned. Please Fix");
            return;
        }

        if (_isLoading)
        {
            Debug.LogError($"Its already loading");
            return;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneLoader.Name));

        if (SceneManager.GetActiveScene().name != _sceneLoader.Name)
        {
            Debug.LogError("Loading Scene not set as Active Scene. Ensure its working correctly");
            return;
        }
        _isLoading = true;
        EnableLoadingCanvas();
        UpdateLoadProgress(0f);
        OnToggleLoadEvent.RaiseEvent(true);
        _currentScene = index;
        UnloadSceneGroup();
    }

    private void UnloadSceneGroup()
    {
        StartCoroutine(UnloadScenes());
    }

    private void LoadSceneGroup()
    {
        StartCoroutine(LoadScenes(_sceneGroups[_currentScene]));
    }

    void EnableLoadingCanvas()
    {
        _loadingCanvas.gameObject.SetActive(true);
        _loadingCamera.SetActive(true);
    }

    void DisableLoadingCanvas()
    {
        _loadingCanvas.gameObject.SetActive(false);
        _loadingCamera.SetActive(false);
    }

    private void FinishedLoading()
    {
        DisableLoadingCanvas();
        _isLoading = false;
        OnFinishedLoadingEvent.RaiseEvent();
        OnScreenFadeInEvent.RaiseEvent(1.5f);
    }

    private void UpdateLoadProgress(float value)
    {
        OnLoadProgressEvent.RaiseEvent(value);
    }

    private IEnumerator UnloadScenes()
    {
        _scenesToUnload = 0;
        var scenes = new List<string>();

        var activeScene = SceneManager.GetActiveScene().name;

        int sceneCount = SceneManager.sceneCount;

        for (int i = sceneCount - 1; i > 0; i--)
        {
            var sceneAt = SceneManager.GetSceneAt(i);

            if (!sceneAt.isLoaded) continue;

            var sceneName = sceneAt.name;

            if (sceneName == activeScene) continue;

            scenes.Add(sceneName);
            _scenesToUnload++;
        }

        UpdateLoadingBarOnUnload();

        foreach (var scene in scenes)
        {
            if (IsAddressableSceneLoaded(scene))
            {
                AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(_addressableScenes[scene]);
                _addressableUnloadOperations.Add(unloadHandle);
                StartCoroutine(WaitForUnloadCompletion(unloadHandle));
            }
            else
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);
                _sceneManagerUnloadOperations.Add(unloadOperation);
                StartCoroutine(WaitForUnloadCompletion(unloadOperation));
            }
        }

        while (!AreAllUnloadingOperationsComplete())
        {
            yield return null;
        }

        LoadSceneGroup();
    }

    private IEnumerator LoadScenes(SceneGroup group)
    {
        ActiveSceneGroup = group;

        _scenesToLoad = ActiveSceneGroup.SubScenes.Count + 1;

        //Load Main Scene
        var scene = group.Scene;
        LoadScene(scene);

        // Load Sub Scenes
        for (int i = 0; i < group.SubScenes.Count; i++)
        {
            var sceneData = group.SubScenes[i];
            LoadScene(sceneData);
        }

        while (!AreAllLoadingOperationsComplete())
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.Name));

        yield return Helpers.GetWaitForSeconds(1f);

        FinishedLoading();
    }

    private void LoadScene(SceneData sceneData)
    {
        if (sceneData.SceneReference.State == SceneReferenceState.Regular)
        {
            AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(sceneData.SceneReference.Path, LoadSceneMode.Additive);
            _sceneManagerLoadOperations.Add(LoadOperation);
            StartCoroutine(WaitForLoadCompletion(LoadOperation));
        }
        else if (sceneData.SceneReference.State == SceneReferenceState.Addressable)
        {
            AsyncOperationHandle<SceneInstance> loadHandle = Addressables.LoadSceneAsync(sceneData.SceneReference.Path, LoadSceneMode.Additive);
            _addressableLoadOperations.Add(loadHandle);
            StartCoroutine(WaitForLoadCompletion(loadHandle));
        }
    }

    // Check if all unloading operations are completed
    private bool AreAllUnloadingOperationsComplete()
    {
        // If there are no active unload operations, return true
        return _sceneManagerUnloadOperations.Count == 0 && _addressableUnloadOperations.Count == 0;
    }

    // Check if all unloading operations are completed
    private bool AreAllLoadingOperationsComplete()
    {
        // If there are no active unload operations, return true
        return _sceneManagerLoadOperations.Count == 0 && _addressableLoadOperations.Count == 0;
    }

    // Coroutine to wait for the unloading operation to complete (for SceneManager)
    private IEnumerator WaitForUnloadCompletion(AsyncOperation unloadOperation)
    {
        while (!unloadOperation.isDone)
        {
            yield return null;
        }
        _sceneManagerUnloadOperations.Remove(unloadOperation);  // Remove the operation once it's complete
        _scenesUnloaded++;
        OnUnloadScene.Invoke();
    }

    // Coroutine to wait for the unloading operation to complete (for Addressables)
    private IEnumerator WaitForUnloadCompletion(AsyncOperationHandle<SceneInstance> unloadHandle)
    {
        while (!unloadHandle.IsDone)
        {
            yield return null;
        }
        _addressableUnloadOperations.Remove(unloadHandle);  // Remove the operation once it's complete
        _scenesUnloaded++;
        OnUnloadScene.Invoke();
    }

    private IEnumerator WaitForLoadCompletion(AsyncOperation loadOperation)
    {
        while (!loadOperation.isDone)
        {
            yield return null;
        }
        _sceneManagerLoadOperations.Remove(loadOperation);
        _scenesLoaded++;
        OnLoadScene.Invoke();
    }

    // Coroutine to wait for the unloading operation to complete (for Addressables)
    private IEnumerator WaitForLoadCompletion(AsyncOperationHandle<SceneInstance> loadHandle)
    {
        while (!loadHandle.IsDone)
        {
            yield return null;
        }
        _addressableLoadOperations.Remove(loadHandle);
        _scenesLoaded++;
        OnLoadScene.Invoke();
    }

    // Check if a scene is loaded via Addressables
    private bool IsAddressableSceneLoaded(string sceneAddress)
    {
        return _addressableScenes.ContainsKey(sceneAddress);
    }

    private void UpdateLoadingBarOnUnload()
    {
        float progress = 0;
        if (_scenesToUnload > 0)
        {
            progress = _scenesUnloaded * (_unloadProgress / _scenesToUnload);
        }
        else
        {
            progress = _unloadProgress;
        }
        UpdateLoadProgress(progress);
    }

    private void UpdateLoadingBarOnLoad()
    {
        float progress = _unloadProgress;
        if (_scenesToLoad > 0)
        {
            progress = _unloadProgress + _scenesLoaded * ((1f - _unloadProgress) / _scenesToLoad);
        }
        else
        {
            progress = 1f;
        }
        UpdateLoadProgress(progress);
    }
}
