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

        if (!enabled || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (!enabled || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    
    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn called");
        yield return FadeText(textTMP, 0f, 1f);
    }

    IEnumerator FadeOut()
    {
        Debug.Log("FadeOut called");
        yield return FadeText(textTMP, 1f, 0f);
    }

    IEnumerator FadeText(TMP_Text text, float start, float end)
    {
        var t = 0f;

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
