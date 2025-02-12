using System;
using Cysharp.Text;
using DamageNumbersPro;
using TMPro;
using UnityEngine;

public class GemTextFeedback : TextPopUp
{
    [SerializeField] private DamageNumber _numberPrefab;
    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private DoubleGameEventListener _currencyGainEventListener;
    [SerializeField] private StringGameEventBinding _currencyGainEventBinding;

    private Transform _transform;
    private Action<string> SpawnPopUpAction;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _numberPrefab.PrewarmPool();
        SpawnPopUpAction = SpawnPopUp;
    }

    private void OnEnable()
    {
        _currencyGainEventBinding.Bind(SpawnPopUpAction, this);
    }

    private void OnDisable()
    {
        _currencyGainEventBinding.Unbind(SpawnPopUpAction, this);

    }

    protected override void SpawnPopUp(string text)
    {
        DamageNumber damageNumber = _numberPrefab.Spawn((Vector2)_transform.position + _positionOffset, text);
    }

    protected override void SpawnPopUp(double text)
    {
        DamageNumber damageNumber = _numberPrefab.Spawn((Vector2)_transform.position + _positionOffset, FormatNumber.FormatDouble(text).GetFormatNoDecimals());
    }
}
