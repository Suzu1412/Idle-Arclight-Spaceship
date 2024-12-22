using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class SceneGroup
{
    public string GroupName = "New Scene Group";
    public SceneData Scene;
    public List<SceneData> SubScenes;

    public string FindSubSceneNameByType(SceneType sceneType)
    {
        return SubScenes.FirstOrDefault(subScene => subScene.SceneType == sceneType)?.Name;
    }
}
