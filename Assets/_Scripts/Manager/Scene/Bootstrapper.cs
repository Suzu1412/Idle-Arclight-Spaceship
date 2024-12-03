using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Bootstrapper : Singleton<Bootstrapper>
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Addressables.LoadSceneAsync("Assets/Scenes/Scene Loader.unity", LoadSceneMode.Single);
    }

}
