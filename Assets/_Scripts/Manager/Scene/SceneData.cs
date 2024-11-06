using Eflatun.SceneReference;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    [SerializeField] private SceneReference _sceneReference;
    [SerializeField] private SceneType _sceneType;

    public string Name => _sceneReference.Name;
    public SceneReference SceneReference => _sceneReference;
    public SceneType SceneType => _sceneType;
}
