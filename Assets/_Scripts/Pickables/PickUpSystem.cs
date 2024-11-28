using System.Collections;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    private IAgent agent;
    public IAgent Agent => agent;
    private bool _isPicked = false;
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
            yield return Helpers.GetWaitForSeconds(0.1f);
            RaycastHit2D hit = Physics2D.CircleCast(_transform.position, _pickUpRadius.Value, Vector2.zero, Mathf.Infinity, _targetLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out ItemPickUp item))
                {
                    item.PickUp(agent);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_transform.position, _pickUpRadius.Value);
    }
}
