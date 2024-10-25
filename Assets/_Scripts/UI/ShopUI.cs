using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private GameObject _shopItemButtonPrefab;
    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private int amountToBuy = 1;
    //private List<>

    public event UnityAction<GeneratorSO> OnBuyGenerator;

    protected override void Awake()
    {
        _currencyManager = CurrencyManager.Instance;
    }

    private void OnEnable()
    {
        _currencyManager.OnGeneratorLoad += PrepareUI;
    }

    private void OnDisable()
    {
        _currencyManager.OnGeneratorLoad -= PrepareUI;
    }

    private void PrepareUI(List<GeneratorSO> generators)
    {
        for(int i=0; i < generators.Count; i++)
        {
            GeneratorButtonController button = Instantiate(_shopItemButtonPrefab).GetComponent<GeneratorButtonController>();
            button.transform.SetParent(_shopItemParent, false);
            button.SetGenerator(generators[i]);
            generators[i].GetBulkCost(amountToBuy);
            generators[i].GetProductionRate();
            button.OnBuyGeneratorClicked += BuyGenerator;
            button.PrepareButton();
        }
    }

    private void BuyGenerator(GeneratorSO generator)
    {
        OnBuyGenerator?.Invoke(generator);
    }

}
