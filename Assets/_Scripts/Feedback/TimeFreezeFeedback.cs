using System.Collections;
using UnityEngine;

public class TimeFreezeFeedback : Feedback
{
    [SerializeField] private float _duration = 0.1f;
    private IHealthSystem _healthSystem;
    internal IHealthSystem HealthSystem => _healthSystem ??= GetComponentInParent<IHealthSystem>();

    private void OnEnable()
    {
        HealthSystem.OnHit += StartFeedback;
    }

    private void OnDisable()
    {
        HealthSystem.OnHit -= StartFeedback;
        ResetFeedback();
    }

    public override void ResetFeedback()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
    }

    public override void StartFeedback()
    {
        StartCoroutine(FreezeTimeCoroutine());
    }

    private IEnumerator FreezeTimeCoroutine()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(_duration);
        Time.timeScale = 1;
    }
}
