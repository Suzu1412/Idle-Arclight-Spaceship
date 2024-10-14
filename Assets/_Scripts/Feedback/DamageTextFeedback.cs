using TMPro;
using UnityEngine;

public class DamageTextFeedback : TextPopUp
{
    [SerializeField] private ObjectPoolSettingsSO _textData;
    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private bool _isPositionRandom = false;
    private Transform _transform;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        HealthSystem.OnDamaged += SpawnPopUp;
    }

    private void OnDisable()
    {
        HealthSystem.OnDamaged -= SpawnPopUp;
    }

    protected override void SpawnPopUp(int text)
    {
        var _popUpGameObject = ObjectPoolFactory.Spawn(_textData).gameObject;
        if (_isPositionRandom)
        {
            SetRandomPosition(_popUpGameObject);
        }
        else
        {
            SetPosition(_popUpGameObject);
        }


        var _popUp = _popUpGameObject.GetComponentInChildren<TextMeshPro>();
        _popUp.text = $"- {text}";
    }

    private void SetPosition(GameObject popUp)
    {
        popUp.transform.position = (Vector2)_transform.position + _positionOffset;
    }

    private void SetRandomPosition(GameObject popUp)
    {
        Vector2 randomPosition = Vector2.zero;
        randomPosition.x = Random.Range(-_positionOffset.x, _positionOffset.x);
        randomPosition.y = Random.Range(-_positionOffset.y, _positionOffset.y);

        popUp.transform.position = (Vector2)_transform.position + randomPosition;
    }
}
