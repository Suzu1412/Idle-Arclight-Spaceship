using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BoolGameEventListener OnToggleShopEventListener;


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
        Debug.Log($"Shop Is Active: {isActive}");
    }





}
