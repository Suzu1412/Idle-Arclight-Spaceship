using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/GeneratorSO")]
public class GeneratorSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private double _baseCost;
    [SerializeField] private double _generationRate;
    [SerializeField] private int _amount;
    [SerializeField] private double _growthRate;

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost => _baseCost;
    public double GenerationRate => _generationRate;
    public int Amount => _amount;
    public double GrowRate => _growthRate;

    public void SetAmount(int amount)
    {
        _amount = amount;
    }
}
