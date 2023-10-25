using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    private TextMeshProUGUI textToDisappear;
    private float timer = 4f; //
    private float fadeDuration = 1f; // 1 second to fade out
    private bool isFading = false;

    void Start()
    {
        textToDisappear = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !isFading)
            {
                isFading = true;
                StartCoroutine(FadeText());
            }
        }
    }

    System.Collections.IEnumerator FadeText()
    {
        float startAlpha = textToDisappear.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            Color newColor = new Color(textToDisappear.color.r, textToDisappear.color.g, textToDisappear.color.b, Mathf.Lerp(startAlpha, 0, t));
            textToDisappear.color = newColor;
            yield return null;
        }

        textToDisappear.color = new Color(textToDisappear.color.r, textToDisappear.color.g, textToDisappear.color.b, 0);
        textToDisappear.enabled = false; // Optional: disable the text component after fading
    }
}
