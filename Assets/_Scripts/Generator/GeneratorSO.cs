using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/GeneratorSO")]
public class GeneratorSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private double _baseCost;
    [SerializeField] private double _generationRate;
    [SerializeField] private int _currentAmount;
    [SerializeField] private double _growthRate;

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost => _baseCost;
    public double GenerationRate => _generationRate;
    public int CurrentAmount => _currentAmount;
    public double GrowRate => _growthRate;


}
