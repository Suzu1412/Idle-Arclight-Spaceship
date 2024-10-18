using System.Collections;
using UnityEngine;

public class DisableOnAnimationEnd : MonoBehaviour
{
    private Animator _animator;
    private ObjectPooler _pool;
    private float _delay;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _pool = GetComponent<ObjectPooler>();
    }


    private void OnEnable()
    {
        StartCoroutine(DisableOnAnimationEndCoroutine());
    }

    private IEnumerator DisableOnAnimationEndCoroutine()
    {
        yield return new WaitForEndOfFrame();
        _delay = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return Helpers.GetWaitForSeconds(_delay);

        if (_pool != null)
        {
            ObjectPoolFactory.ReturnToPool(_pool);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }


}
