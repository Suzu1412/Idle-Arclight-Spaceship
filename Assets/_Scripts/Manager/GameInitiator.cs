using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using System.Collections.Generic;

public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Scene References")]
    [SerializeField] private SceneReference _bootstrapperScene;

    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnStartGameEvent;

    private void OnEnable()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        bool isValid = false;

        while (!isValid)
        {

            if (SceneManager.GetActiveScene().name != _bootstrapperScene.Name)
            {
                isValid = true;
                break;
            }

            yield return Helpers.GetWaitForSeconds(0.1f);
        }

        OnStartGameEvent.RaiseEvent();
    }
}
