using UnityEngine;

public interface IPausable
{
    BoolVariableSO IsPaused { get; set; }

    void Pause(bool isPaused);

    GameObject GetGameObject();
}
