using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class FlashTransparentFeedback : Feedback
{
    private IAgent _agent;
    [SerializeField] private float _feedbackTime = 0.1f;
    [SerializeField] private float _targetAlpha = 0.1f;

    internal IAgent Agent => _agent ??= GetComponentInParent<IAgent>();

    private Coroutine _feedbackCoroutine;

    private void OnEnable()
    {
        Agent.HealthSystem.OnInvulnerabilityPeriod += StartFeedback;
    }

    private void OnDisable()
    {
        Agent.HealthSystem.OnInvulnerabilityPeriod -= StartFeedback;
    }

    private IEnumerator FeedbackCoroutine()
    {
        float invulnerabilityDuration = Agent.HealthSystem.InvulnerabilityDuration;

        Agent.AgentRenderer.TransparencyOverTime(_targetAlpha, _feedbackTime);

        yield return Helpers.GetWaitForSeconds(invulnerabilityDuration);
        Agent.AgentRenderer.TransparencyOverTime(1f, _feedbackTime);

    }

    private IEnumerator BlinkCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            Agent.AgentRenderer.SpriteRenderer.enabled = false;
            yield return Helpers.GetWaitForSeconds(0.1f);
            Agent.AgentRenderer.SpriteRenderer.enabled = true;
            yield return Helpers.GetWaitForSeconds(0.2f);
        }
        for (int i = 0; i < 2; i++)
        {
            Agent.AgentRenderer.SpriteRenderer.enabled = false;
            yield return Helpers.GetWaitForSeconds(0.1f);
            Agent.AgentRenderer.SpriteRenderer.enabled = true;
            yield return Helpers.GetWaitForSeconds(0.05f);
        }


        Agent.AgentRenderer.TransparencyOverTime(1f, _feedbackTime);
    }

    public override void StartFeedback()
    {
        ResetFeedback();
        _feedbackCoroutine = StartCoroutine(FeedbackCoroutine());
    }

    public override void ResetFeedback()
    {
        if (_feedbackCoroutine != null) StopCoroutine(_feedbackCoroutine);
        Agent.AgentRenderer.TransparencyOverTime(1f, 0.001f);

    }
}
