using UnityEngine;

public interface IPausable
{
    WaitUntilSO WaitUntil { get; set; }
    BoolVariableSO IsPaused { get; set; }

    void Pause(bool isPaused);

    GameObject GetGameObject();
}
