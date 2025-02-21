using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Canvas _shopCanvas;
    [SerializeField] private BoolGameEventBinding OnToggleShopMenuListener;

    private void Start()
    {
        _shopCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnToggleShopMenuListener.Bind(ToggleShop, this);
    }

    private void OnDisable()
    {
        OnToggleShopMenuListener.Unbind(ToggleShop, this);
    }

    private void ToggleShop(bool isEnabled)
    {
        _shopCanvas.gameObject.SetActive(isEnabled);

    }

}
