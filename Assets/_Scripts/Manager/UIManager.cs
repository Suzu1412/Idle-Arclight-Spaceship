using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private BoolGameEventListener OnToggleShopEventListener;

    [Header("Shop UI")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopDefaultButton;

    private void OnEnable()
    {
        OnToggleShopEventListener.Register(ToggleShop);
    }

    private void OnDisable()
    {
        OnToggleShopEventListener.DeRegister(ToggleShop);
    }


    private void ToggleShop(bool isActive)
    {
        _shopUI.SetActive(isActive);
        if (isActive)
        {
            _shopDefaultButton.Select();
        }
    }





}
