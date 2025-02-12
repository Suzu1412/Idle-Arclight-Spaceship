using System;
using UnityEngine;

public class UnlockSystem : MonoBehaviour
{
    [SerializeField] private UnlockedSystemSO _unlockedSystem;
    [SerializeField] private VoidGameEventBinding OnUnlockSystemBinding;
    private Action UnlockAction;


    private void Awake()
    {
        UnlockAction = Unlock;

        if (OnUnlockSystemBinding != null)
        {
            OnUnlockSystemBinding.Bind(UnlockAction, this);

        }
        else
        {
            Debug.LogWarning($"{name} is missing an OnUnlockSystemBinding reference!", this);
        }

    }

    private void OnDestroy()
    {
        if (OnUnlockSystemBinding != null)
        {
            OnUnlockSystemBinding.Unbind(UnlockAction, this);

        }
    }

    private void Unlock()
    {
        this.gameObject.SetActive(_unlockedSystem.IsUnlocked);
    }

}
