using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickUpSystem : MonoBehaviour
{
    private IAgent agent;
    public IAgent Agent => agent;
    private bool _isColliding = false;
    private Coroutine _resetCoroutine;


    private void Awake()
    {
        agent = GetComponent<IAgent>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isColliding) return;

        if (collision.TryGetComponent(out ItemPickUp item))
        {
            item.PickUp(agent);
            _resetCoroutine = StartCoroutine(ResetCoroutine());
        }
    }

    private IEnumerator ResetCoroutine()
    {
        _isColliding = true;
        yield return new WaitForEndOfFrame();
        _isColliding = false;
    }
}
