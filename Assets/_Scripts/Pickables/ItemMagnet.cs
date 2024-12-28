using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    private ItemPickUp _pickUp;
    private Collider2D _collider;


    internal ItemPickUp PickUp => _pickUp != null ? _pickUp : _pickUp = GetComponentInParent<ItemPickUp>();

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }


    public void Magnet(Transform other)
    {
        PickUp.Magnet(other);
        _collider.enabled = false;
    }

}
