using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
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

    public void SetShopDefaultButton()
    {
        _shopDefaultButton.Select();
    }

    public bool CheckIfButtonIsSelected(GameObject button)
    {
        Debug.Log("trying to check selected");

        if (EventSystem.current.currentSelectedGameObject == button)
        {
            return true;
        }
        return false;
    }

    public void DeselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ToggleShop(bool isActive)
    {
        _shopUI.SetActive(isActive);
        if (isActive)
        {
            SetShopDefaultButton();
        }
    }


}
