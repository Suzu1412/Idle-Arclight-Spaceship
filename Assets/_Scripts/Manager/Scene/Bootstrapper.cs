using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async void Init()
    {
        Debug.Log("Bootstrapper...");
        await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
    }

}
