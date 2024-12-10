using UnityEditor.VersionControl;
using UnityEngine;

public interface INotification 
{
    public string Message { get; }

    public float Duration { get; }

    public float Amount { get; }
    public float Multiplier { get; }
    public Sprite Sprite { get; }

    public void SetMessage(string message);

    public void SetDuration(float duration);
    public void SetAmount(float amount);

    public void SetMultiplier(float multiplier);

    public void SetSprite(Sprite sprite);
}
