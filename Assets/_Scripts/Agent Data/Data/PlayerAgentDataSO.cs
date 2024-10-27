using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAgentDataSO", menuName = "Scriptable Objects/PlayerAgentDataSO")]
public class PlayerAgentDataSO : SerializableScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private StatsSO _baseStats;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _currentExp;

    public Sprite Sprite => _sprite;
    public StatsSO BaseStats => _baseStats;
    public int CurrentHealth => _currentHealth;
    public float CurrentExp => _currentExp;

    public void SetCurrentHealth(int currentHealth)
    {
        _currentHealth = currentHealth;
    }

    public void SetCurrentExp(float currentExp)
    {
        _currentExp = currentExp;
    }
}
