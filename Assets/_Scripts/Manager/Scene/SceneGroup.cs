using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class SceneGroup
{
    public string GroupName = "New Scene Group";
    public List<SceneData> Scenes;

    public string FindSceneNameByType(SceneType sceneType)
    {
        return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Name;
    }
}
