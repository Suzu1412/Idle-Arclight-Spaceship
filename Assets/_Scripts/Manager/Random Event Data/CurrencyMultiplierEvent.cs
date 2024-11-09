using UnityEngine;

[System.Serializable]
public class CurrencyMultiplierEvent : IRandomEvent
{
    public void ActivateEvent()
    {
        Debug.Log("Activando evento!");
    }

    public void DeactivateEvent()
    {
    }
}
