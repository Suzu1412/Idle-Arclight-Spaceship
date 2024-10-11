using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGameObjectRunTimeSet : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSetSO _gameObjectRunTimeSet;

    private void OnEnable()
    {
        _gameObjectRunTimeSet.Add(gameObject);
    }

    private void OnDisable()
    {
        _gameObjectRunTimeSet.Remove(gameObject);
    }
}
