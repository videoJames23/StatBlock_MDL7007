using System.Collections;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    private TMP_Text textTMP;
    void Start()
    {
        textTMP = GetComponent<TMP_Text>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    
    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn called");
        yield return FadeText(textTMP, 0f, 1f);
        StopCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        Debug.Log("FadeOut called");
        yield return FadeText(textTMP, 1f, 0f);
        StopCoroutine(FadeOut());
    }

    IEnumerator FadeText(TMP_Text text, float start, float end)
    {
        float t = 0f;

        while (t < 1f)
        {
            float a = Mathf.Lerp(start, end, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, a);
            t += Time.deltaTime;
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, end);
    }
    
}
