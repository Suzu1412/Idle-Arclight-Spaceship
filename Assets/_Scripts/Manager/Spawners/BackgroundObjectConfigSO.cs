using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundObjectSO", menuName = "Scriptable Objects/Spawner/Background Object SO")]
public class BackgroundObjectConfigSO : ScriptableObject
{
    [SerializeField] private ObjectPoolSettingsSO _poolSettings;
    [SerializeField] private List<Sprite> _spriteList;
    [SerializeField] private List<BackgroundObjectConfiguration> _configurationList;

    public ObjectPoolSettingsSO PoolSettings => _poolSettings;
    public List<Sprite> SpriteList => _spriteList;
    public List<BackgroundObjectConfiguration> ConfigurationList => _configurationList;
}

[System.Serializable]
public class BackgroundObjectConfiguration
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private int _sortingOrder;

    public float MoveSpeed => _moveSpeed;
    public Vector3 Scale => _scale;
    public int SortingOrder => _sortingOrder;

}
