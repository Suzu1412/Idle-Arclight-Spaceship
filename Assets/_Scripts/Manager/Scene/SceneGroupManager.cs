using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneGroupManager
{
    public float delayTightLoad = 0.5f;

    public event UnityAction<string> OnSceneLoaded;
    public event UnityAction<string> OnSceneUnloaded;
    public event UnityAction OnSceneGroupLoaded;
    public event UnityAction OnSceneGroupUnloaded;

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

            var operation = SceneManager.LoadSceneAsync(sceneData.SceneReference.Path, LoadSceneMode.Additive);

            // TODO: Remove
            //await Awaitable.WaitForSecondsAsync(2.5f);

            operationGroup.Operations.Add(operation);

            OnSceneLoaded?.Invoke(sceneData.Name);

        }

        while (!operationGroup.IsDone)
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

        while (!OperationGroup.IsDone)
        {
            await Awaitable.WaitForSecondsAsync(delayTightLoad);
        }

        OnSceneGroupUnloaded?.Invoke();
    }
}

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