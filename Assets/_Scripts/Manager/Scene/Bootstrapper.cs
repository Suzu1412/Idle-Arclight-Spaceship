using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : Singleton<Bootstrapper>
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async void Init()
    {
        if (SceneManager.GetActiveScene().name == "Bootstrapper")
        {
            return;
        }

        await SceneManager.LoadSceneAsync("Scene Loader", LoadSceneMode.Single);
    }

}
