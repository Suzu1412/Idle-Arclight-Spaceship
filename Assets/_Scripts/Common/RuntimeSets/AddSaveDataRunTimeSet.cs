using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSaveDataRunTimeSet : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSetSO _saveDataRunTimeSet = default;

    private void OnEnable()
    {
        _saveDataRunTimeSet.Add(gameObject);
    }

    private void OnDisable()
    {
        _saveDataRunTimeSet.Remove(gameObject);
    }
}
