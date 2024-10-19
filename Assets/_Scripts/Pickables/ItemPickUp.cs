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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float animationDuration = 0.3f;
    private ObjectPooler _pool;
    public ObjectPooler Pool => _pool = _pool != null ? _pool : gameObject.GetOrAdd<ObjectPooler>();

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _pickableCollider = GetComponent<CircleCollider2D>();
    }

    public void SetItem(ItemSO item)
    {
        _item = item;
        _spriteRenderer.sprite = item.ItemImage;
    }

    public void PickUp(IAgent agent)
    {
        _item.PickUp(agent);
        DestroyItem();
    }

    private void DestroyItem()
    {
        _pickableCollider.enabled = false;
        StartCoroutine(AnimateItemPickUp());

    }

    private IEnumerator AnimateItemPickUp()
    {
        if (audioSource != null) audioSource.Play();

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
        Gizmos.DrawWireSphere(_pickableCollider.transform.position, _pickableCollider.radius);
    }
}
