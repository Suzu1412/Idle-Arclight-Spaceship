using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MaterialBrightness : MonoBehaviour
{
    private Material g;
    [SerializeField] private float _baseGlow = 1f;
    [SerializeField] private float _targetGlow = 5;
    [SerializeField] private float _duration = 2f;

    private void Start()
    {
        g = GetComponent<SpriteRenderer>().material;


    }

    void OnEnable()
    {
        if (g == null)
        {
            g = GetComponent<SpriteRenderer>().material;
        }

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(g.DOFloat(_targetGlow, "_GlowAmount", _duration));
        mySequence.PrependInterval(0.5f);
        mySequence.Append(g.DOFloat(_baseGlow, "_GlowAmount", _duration));
        mySequence.PrependInterval(0.5f);
        mySequence.SetLoops(-1);
       
        //g.SetFloat("_GlowAmount", _baseGlow);
        //Debug.Log("starting new on enable");
        //.SetLoops(-1, LoopType.Yoyo);

    }
}
