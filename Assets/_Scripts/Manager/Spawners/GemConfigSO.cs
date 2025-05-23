using UnityEngine;

[CreateAssetMenu(fileName = "GemConfigSO", menuName = "Scriptable Objects/Spawner/GemConfigSO")]
public class GemConfigSO : ScriptableObject
{
    [SerializeField] private ObjectPoolSettingsSO _poolSettings;
    [SerializeField] private ItemSO _item;
    [SerializeField] private float _spawnChance;
    [SerializeField] private float _moveSpeed;

    public ObjectPoolSettingsSO PoolSettings => _poolSettings;
    public ItemSO Item => _item;
    public float spawnChance => _spawnChance;
    public float moveSpeed => _moveSpeed;
}
