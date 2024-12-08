using System.Collections;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    private IAgent agent;
    public IAgent Agent => agent;
    [SerializeField] private FloatVariableSO _pickUpRadius;
    [SerializeField] private LayerMask _targetLayer;
    private Transform _transform;
    private Coroutine _pickUpCoroutine;


    private void Awake()
    {
        agent = GetComponent<IAgent>();
        _transform = transform;
    }

    private void OnEnable()
    {
        _pickUpCoroutine = StartCoroutine(PickUpCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_pickUpCoroutine);
    }

    private IEnumerator PickUpCoroutine()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(0.05f);
            Collider2D collider = Physics2D.OverlapCircle(_transform.position, _pickUpRadius.Value, _targetLayer);

            if (collider != null)
            {
                if (collider.TryGetComponent(out ItemPickUp item))
                {
                    item.Magnet(this.transform);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_pickUpRadius == null) return;
        Gizmos.DrawWireSphere(transform.position, _pickUpRadius.Value);
    }
}
