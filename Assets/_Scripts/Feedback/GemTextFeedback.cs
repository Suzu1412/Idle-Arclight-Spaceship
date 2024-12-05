using TMPro;
using UnityEngine;

public class GemTextFeedback : TextPopUp
{
    [SerializeField] private ObjectPoolSettingsSO _textData;
    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private DoubleGameEventListener _currencyGainEventListener;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        _currencyGainEventListener.Register(SpawnPopUp);
    }

    private void OnDisable()
    {
        _currencyGainEventListener.DeRegister(SpawnPopUp);

    }

    protected override void SpawnPopUp(double text)
    {
        var _popUpGameObject = ObjectPoolFactory.Spawn(_textData).gameObject;
        SetPosition(_popUpGameObject);

        var _popUp = _popUpGameObject.GetComponentInChildren<TextMeshPro>();
        _popUp.text = $"+ {FormatNumber.FormatDouble(text).GetFormatNoDecimals()}";
    }

    private void SetPosition(GameObject popUp)
    {
        popUp.transform.position = (Vector2)_transform.position + _positionOffset;
    }
}
