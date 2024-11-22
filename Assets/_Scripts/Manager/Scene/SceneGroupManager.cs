using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class SceneGroupManager
{
    public float delayTightLoad = 0.5f;

    public event UnityAction<string> OnSceneLoaded;
    public event UnityAction<string> OnSceneUnloaded;
    public event UnityAction OnSceneGroupLoaded;
    public event UnityAction OnSceneGroupUnloaded;

    private readonly AsyncOperationHandleGroup _handleGroup = new AsyncOperationHandleGroup(10);

    SceneGroup ActiveSceneGroup;

    public async Awaitable LoadScenes(SceneGroup group, bool reloadDupscenes = false)
    {
        ActiveSceneGroup = group;
        var loadedScenes = new List<string>();

        await UnloadScenes();
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

        var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

        for (int i = 0; i < totalScenesToLoad; i++)
        {
            var sceneData = group.Scenes[i];

            if (reloadDupscenes == false && loadedScenes.Contains(sceneData.Name)) continue;

            if (sceneData.SceneReference.State == SceneReferenceState.Regular)
            {
                var operation = SceneManager.LoadSceneAsync(sceneData.SceneReference.Path, LoadSceneMode.Additive);
                operationGroup.Operations.Add(operation);
            }
            else if (sceneData.SceneReference.State == SceneReferenceState.Addressable)
            {
                var sceneHandle = Addressables.LoadSceneAsync(sceneData.SceneReference.Path, LoadSceneMode.Additive);
                _handleGroup.Handles.Add(sceneHandle);
            }

            OnSceneLoaded?.Invoke(sceneData.Name);

        }

        while (!operationGroup.IsDone || !_handleGroup.IsDone)
        {
            await Awaitable.WaitForSecondsAsync(delayTightLoad);
        }

        Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

        if (activeScene.IsValid())
        {
            SceneManager.SetActiveScene(activeScene);
        }

        OnSceneGroupLoaded?.Invoke();

    }

    public async Awaitable UnloadScenes()
    {
        var scenes = new List<string>();

        var activeScene = SceneManager.GetActiveScene().name;

        int sceneCount = SceneManager.sceneCount;

        for (int i = sceneCount - 1; i > 0; i--)
        {
            var sceneAt = SceneManager.GetSceneAt(i);

            if (!sceneAt.isLoaded) continue;

            var sceneName = sceneAt.name;

            if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
            if (_handleGroup.Handles.Any(h => h.IsValid() && h.Result.Scene.name == sceneName)) continue;

            scenes.Add(sceneName);
        }

        var OperationGroup = new AsyncOperationGroup(scenes.Count);

        foreach (var scene in scenes)
        {
            var operation = SceneManager.UnloadSceneAsync(scene);

            if (operation == null) continue;

            OperationGroup.Operations.Add(operation);

            OnSceneUnloaded?.Invoke(scene);
        }

        foreach (var handle in _handleGroup.Handles)
        {
            if (handle.IsValid())
            {
                Addressables.UnloadSceneAsync(handle);
            }
        }

        _handleGroup.Handles.Clear();

        while (!OperationGroup.IsDone || !_handleGroup.IsDone)
        {
            await Awaitable.WaitForSecondsAsync(delayTightLoad);
        }

        OnSceneGroupUnloaded?.Invoke();
    }
}

// load Scenes
public readonly struct AsyncOperationGroup
{
    public readonly List<AsyncOperation> Operations;
    public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
    public bool IsDone => Operations.All(o => o.isDone);

    public AsyncOperationGroup(int initialCapacity)
    {
        Operations = new(initialCapacity);
    }
}

// Load Addressable Scenes
public readonly struct AsyncOperationHandleGroup
{
    public readonly List<AsyncOperationHandle<SceneInstance>> Handles;
    public float Progress => Handles.Count == 0 ? 0 : Handles.Average(o => o.PercentComplete);
    public bool IsDone => Handles.All(o => o.IsDone);

    public AsyncOperationHandleGroup(int initialCapacity)
    {
        Handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
    }
}