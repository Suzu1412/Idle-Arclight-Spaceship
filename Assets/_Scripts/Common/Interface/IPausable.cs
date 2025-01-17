using UnityEngine;

public interface IPausable
{
    BoolVariableSO IsPaused { get; }

    void Pause(bool isPaused);

    GameObject GetGameObject();
}
