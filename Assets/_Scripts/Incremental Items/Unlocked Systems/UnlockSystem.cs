using UnityEngine;

public class UnlockSystem : MonoBehaviour
{
    [SerializeField] private UnlockedSystemSO _unlockedSystem;
    [SerializeField] private VoidGameEventListener OnUnlockSystemListener;


    private void Awake()
    {
        OnUnlockSystemListener.Register(Unlock);
    }

    private void OnDestroy()
    {
        OnUnlockSystemListener.DeRegister(Unlock);
    }

    private void Unlock()
    {
        this.gameObject.SetActive(_unlockedSystem.IsUnlocked);
    }

}
