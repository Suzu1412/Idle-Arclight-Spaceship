using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MaterialBrightness : MonoBehaviour
{
    private Material g;
    [SerializeField] private float _baseGlow = 1f;
    [SerializeField] private float _targetGlow = 5;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _pauseDuration = 0.3f;

    private void Start()
    {
        g = GetComponent<SpriteRenderer>().material;
    }

    void OnEnable()
    {
        StartCoroutine(ChangeGlowCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChangeGlowCoroutine()
    {
        if (g == null)
        {
            g = GetComponent<SpriteRenderer>().material;
        }

        while (true)
        {
            g.DOFloat(_targetGlow, "_GlowAmount", _duration);
            yield return Helpers.GetWaitForSeconds(_pauseDuration);
            g.DOFloat(_baseGlow, "_GlowAmount", _duration);
            yield return Helpers.GetWaitForSeconds(_pauseDuration);
        }
    }
}
