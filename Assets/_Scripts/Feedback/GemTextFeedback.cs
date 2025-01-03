using Cysharp.Text;
using DamageNumbersPro;
using TMPro;
using UnityEngine;

public class GemTextFeedback : TextPopUp
{
    [SerializeField] private DamageNumber numberPrefab;
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
        DamageNumber damageNumber = numberPrefab.Spawn((Vector2)_transform.position + _positionOffset, FormatNumber.FormatDouble(text).GetFormatNoDecimals());
    }

    private void SetPosition(GameObject popUp)
    {
        popUp.transform.position = (Vector2)_transform.position + _positionOffset;
    }
}
