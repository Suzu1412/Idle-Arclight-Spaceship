using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private ObjectPoolSettingsSO _settings;

    internal ObjectPoolSettingsSO Settings => _settings;

    internal void SetSettings(ObjectPoolSettingsSO settings)
    {
        _settings = settings;
    }
}
