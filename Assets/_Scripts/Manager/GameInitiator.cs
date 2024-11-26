using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using System.Collections.Generic;

public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Scene References")]
    [SerializeField] private List<SceneReference> _validActiveScenes;

    [Header("Managers")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private PlayerManager _playerManager;

    [Header("Spawners")]
    [SerializeField] private GemSpawner _gemSpawner;

    private void OnEnable()
    {
        StartCoroutine(StartGame());
    }

    private void BindObjects()
    {
        _playerManager = PlayerManager.Instance;
        _saveSystem = SaveSystem.Instance;
        _currencyManager = CurrencyManager.Instance;
    }

    private void Initialize()
    {
        _playerManager.SpawnPlayer();
    }

    private void LoadGame()
    {
        _saveSystem.LoadGame();
    }

    private IEnumerator StartGame()
    {
        bool isValid = false;

        while (!isValid)
        {
            foreach(var activeScene in _validActiveScenes)
            {
                if (SceneManager.GetActiveScene().name == activeScene.Name)
                {
                    isValid = true; 
                    break;
                }
            }

            yield return Helpers.GetWaitForSeconds(0.1f);
        }

        

        BindObjects();
        Initialize();
        LoadGame();
    }
}
