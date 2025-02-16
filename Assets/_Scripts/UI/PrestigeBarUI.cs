using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrestigeBarUI : MonoBehaviour
{
    [SerializeField] private Slider prestigeSlider;
    [SerializeField] private Text prestigeText;
    [SerializeField] private float animationSpeed = 2f; // Speed of the animation

    private PrestigeManager prestigeManager;
    private Coroutine animationCoroutine;

    private void Start()
    {
        //prestigeManager = FindObjectOfType<PrestigeManager>();
        prestigeManager.OnPrestigeProgressUpdated += AnimateUI;
    }

    private void OnDestroy()
    {
        prestigeManager.OnPrestigeProgressUpdated -= AnimateUI;
    }

    private void AnimateUI(float current, float required)
    {
        float targetProgress = current / required;
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(SmoothUpdate(targetProgress, current, required));
    }

    private IEnumerator SmoothUpdate(float targetValue, float current, float required)
    {
        float startValue = prestigeSlider.value;
        float elapsedTime = 0f;
        float duration = 0.8f; // Duration of animation

        // Handle overflow: If the new progress is lower than the current value, play a reset animation
        if (targetValue < startValue)
        {
            // Step 1: Animate to 100% (full bar)
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * animationSpeed;
                float t = elapsedTime / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                prestigeSlider.value = Mathf.Lerp(startValue, 1f, t);
                yield return null;
            }

            // Step 2: Instantly reset to 0
            prestigeSlider.value = 0f;

            // Small delay to make it feel more fluid
            yield return new WaitForSeconds(0.1f);
        }

        // Step 3: Animate from 0% to the target value
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * animationSpeed;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            prestigeSlider.value = Mathf.Lerp(0f, targetValue, t);
            prestigeText.text = $"{Mathf.Floor(current)}/{Mathf.Floor(required)} Prestige Points";
            yield return null;
        }

        // Ensure exact final value
        prestigeSlider.value = targetValue;
    }
}
