using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyMultiplierSO", menuName = "Scriptable Objects/CurrencyMultiplierSO")]
public class CurrencyMultiplierSO : ScriptableObject
{
    private float _multiplier = 1f;
    public float Multiplier => _multiplier;

    public void SetMultiplier(float multiplier)
    {
        if (multiplier < 1f)
        {
            multiplier = 1f;
        }
        else if(multiplier > 500f)
        {
            multiplier = 500f;
        }

        _multiplier = multiplier;
    }
}
