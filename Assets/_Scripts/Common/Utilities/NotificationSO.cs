using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Notification", fileName = "Notification")]
public class NotificationSO : ScriptableObject, INotification
{
    [SerializeField] private string _message;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private string _amount;
    [SerializeField] private float _multiplier = 0f;
    [SerializeField] private Sprite _sprite;

    public string Message => _message;

    public float Duration => _duration;

    public string Amount => _amount;
    public float Multiplier => _multiplier;
    public Sprite Sprite => _sprite;



    public void SetMessage(string message)
    {
        _message = message;
    }

    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void SetAmount(string amount)
    {
        _amount = amount;
    }

    public void SetMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }

    public void SetSprite(Sprite sprite)
    {
        _sprite = sprite;
    }
}
