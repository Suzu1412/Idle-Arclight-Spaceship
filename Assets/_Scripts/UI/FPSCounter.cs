using System.Collections;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private float count;
    [SerializeField] private TextMeshProUGUI _text;

    private IEnumerator Start()
    {
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            _text.text = "FPS: " + count.ToString();
            yield return Helpers.GetWaitForSeconds(0.1f);
        }
    }
}
