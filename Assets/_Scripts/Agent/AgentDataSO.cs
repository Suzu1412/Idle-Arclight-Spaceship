using UnityEngine;

public abstract class AgentDataSO : SerializableScriptableObject
{
    [SerializeField] protected StatsSO _initialStats;
    [SerializeField] protected MovementDataSO _movementData;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _currentLevel;
    [SerializeField] protected float _totalExp;
    [SerializeField] protected Sprite _sprite;
    
    public StatsSO InitialStats => _initialStats;
    public MovementDataSO MovementData => _movementData;
    public int CurrentHealth => _currentHealth;
    public int CurrentLevel => _currentLevel;
    public Sprite Sprite => _sprite;

    public void SetCurrentHealth(int currentHealth)
    {
        _currentHealth = currentHealth;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        _currentLevel = currentLevel;
    }

    public void SetTotalExp(float totalExp)
    {
        _totalExp = totalExp;
    }
}
