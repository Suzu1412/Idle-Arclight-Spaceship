using TMPro;
using UnityEngine;

public class ExpTextFeedback : TextPopUp
{
    [SerializeField] private ObjectPoolSettingsSO _textData;
    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private bool _isPositionRandom = false;
    private Transform _transform;
    private DeathReward _deathReward;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _deathReward = GetComponentInParent<DeathReward>();

    }

    private void OnEnable()
    {
        _deathReward.OnGiveExp += SpawnPopUp;
    }

    private void OnDisable()
    {
        _deathReward.OnGiveExp -= SpawnPopUp;
    }

    protected override void SpawnPopUp(float text)
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
        _popUp.text = $"+ {text} EXP";
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
