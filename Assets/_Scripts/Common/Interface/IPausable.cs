using UnityEngine;

public interface IPausable
{
    void Pause(bool isPaused);

    GameObject GetGameObject();
}
