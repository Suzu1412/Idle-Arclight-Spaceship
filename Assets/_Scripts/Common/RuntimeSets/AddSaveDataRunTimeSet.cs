using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSaveDataRunTimeSet : MonoBehaviour
{
    [SerializeField] private SaveableRunTimeSetSO _saveDataRunTimeSet = default;

    private void OnEnable()
    {
        _saveDataRunTimeSet.Add(gameObject.GetComponent<ISaveable>());
    }

    private void OnDisable()
    {
        //_saveDataRunTimeSet.Remove(gameObject);
    }
}
