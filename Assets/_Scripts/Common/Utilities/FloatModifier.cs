using UnityEngine;

[System.Serializable]
public class FloatModifier
{
    [SerializeField] private ModifierType _modifierType;
    [SerializeField] private float _value = 1.0f;
    [SerializeField] private string _source;

    public ModifierType ModifierType { get => _modifierType; internal set => _modifierType = value; }
    public float Value { get => _value; internal set => _value = value; }
    public string Source { get => _source; internal set => _source = value; }

    /// </summary>
    /// <param name="modifierType">Type of Modifier</param>
    /// <param name="value">Value: 1.0 or more for multiplicative</param>
    /// <param name="source">Source is the gameobject it comes from </param>
    public FloatModifier(ModifierType modifierType, float value, string source)
    {
        _modifierType = modifierType;
        _value = value;
        _source = source;
    }
}
