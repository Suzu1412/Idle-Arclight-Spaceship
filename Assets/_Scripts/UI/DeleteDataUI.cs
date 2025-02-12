using UnityEngine;

public class DeleteDataUI : MonoBehaviour
{
    [SerializeField] private VoidGameEvent OnDeleteSaveDataEvent;

    public void DeleteSaveData()
    {
        OnDeleteSaveDataEvent.RaiseEvent(this);
    }
}
