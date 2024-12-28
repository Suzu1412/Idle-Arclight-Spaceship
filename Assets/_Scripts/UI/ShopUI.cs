using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Singleton<ShopUI>
{
    [Header("Generator")]
    [SerializeField] private ListGeneratorSO _generators;
    [SerializeField] private GameObject _shopGeneratorButtonPrefab;
    [SerializeField] private Transform _shopGeneratorContent;
    [SerializeField] private GameObject _generatorCanvasGO;
    private List<GeneratorButtonController> _generatorButtons = new();

    [Header("Upgrade")]
    [SerializeField] private ListUpgradeSO _upgrades;
    [SerializeField] private GameObject _shopUpgradeButtonPrefab;
    [SerializeField] private Transform _shopUpgradeContent;
    [SerializeField] private GameObject _UpgradeCanvasGO;
    private List<UpgradeButtonController> _upgradeButtons = new();

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnBuyGeneratorGameEvent;
    [SerializeField] private IntGameEvent OnBuyUpgradeGameEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnGeneratorAmountChangedListener;
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;


    private bool _generatorInitialized;
    private bool _upgradeInitialized;

    protected override void Awake()
    {
        base.Awake();
        transform.localPosition = Vector2.zero;
    }

    protected void Start()
    {
        PrepareUIGenerator(_generators.Generators);
        PrepareUIUpgrade(_upgrades.Upgrades);
    }

    private void OnEnable()
    {
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        _generatorInitialized = false;
        _upgradeInitialized = false;
    }

    private void OnDisable()
    {
        OnGeneratorAmountChangedListener.DeRegister(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
    }

    private void PrepareUIGenerator(List<GeneratorSO> generators)
    {
        _generatorButtons.Clear();

        for (int i = 0; i < generators.Count; i++)
        {
            GeneratorButtonController button = Instantiate(_shopGeneratorButtonPrefab).GetComponent<GeneratorButtonController>();
            button.transform.SetParent(_shopGeneratorContent, false);
            button.SetIndex(i);
            button.SetGenerator(generators[i]);
            generators[i].GetProductionRate();
            button.ChangeAmountToBuy();
            button.OnBuyGeneratorClicked += BuyGenerator;
            button.PrepareButton();
            _generatorButtons.Add(button);
        }

        _generatorInitialized = true;
        CloseCanvas();
    }

    private void PrepareUIUpgrade(List<BaseUpgradeSO> upgrades)
    {
        _upgradeButtons.Clear();

        for (int i = 0; i < upgrades.Count; i++)
        {
            UpgradeButtonController button = Instantiate(_shopUpgradeButtonPrefab).GetComponent<UpgradeButtonController>();
            button.transform.SetParent(_shopUpgradeContent, false);
            button.SetIndex(i);
            button.SetUpgrade(upgrades[i]);
            upgrades[i].GetCost();
            button.OnBuyUpgradeClicked += BuyUpgrade;
            button.PrepareButton();
            _upgradeButtons.Add(button);
        }

        _upgradeInitialized = true;
        CloseCanvas();
    }

    private void CloseCanvas()
    {
        if (_generatorInitialized && _upgradeInitialized)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateButtonInfo(int index)
    {
        foreach(var generatorButton in _generatorButtons)
        {
            generatorButton.PrepareButton();
        }
    }

    private void BuyGenerator(int index)
    {
        OnBuyGeneratorGameEvent.RaiseEvent(index);
    }

    private void BuyUpgrade(int index)
    {
        OnBuyUpgradeGameEvent.RaiseEvent(index);
    }

    private void ChangeAmountToBuy(int amount)
    {
        foreach (var button in _generatorButtons)
        {
            button.ChangeAmountToBuy();
        }
    }
}
