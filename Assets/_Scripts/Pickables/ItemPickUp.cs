using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class ItemPickUp : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _pickableCollider;
    [SerializeField] private ItemSO _item;
    [SerializeField] private Color gizmoColor = Color.magenta;
    [SerializeField] private SoundDataSO _soundOnPickup;
    [SerializeField] private float animationDuration = 0.3f;
    private bool _isPicked;
    private ObjectPooler _pool;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _pickableCollider = GetComponent<CircleCollider2D>();

        if (_pickableCollider == null)
        {
            Debug.Log($"Please Add Collider to: {this.gameObject}");
        }
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        _isPicked = false;
    }

    public void SetItem(ItemSO item)
    {
        _item = item;
        _spriteRenderer.sprite = item.ItemImage;
    }

    public void PickUp(IAgent agent)
    {
        if (_isPicked) return;
        _isPicked = true;
        _item.PickUp(agent);
        DestroyItem();
    }

    private void DestroyItem()
    {
        StartCoroutine(AnimateItemPickUp());
    }

    private IEnumerator AnimateItemPickUp()
    {
        _soundOnPickup?.PlayEvent();

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0f;

        while (currentTime < animationDuration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / animationDuration);
            yield return null;
        }

        transform.localScale = endScale;
        ObjectPoolFactory.ReturnToPool(Pool);
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _pickableCollider.radius);
    }
}
