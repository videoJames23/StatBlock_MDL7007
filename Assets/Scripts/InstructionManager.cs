using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    private GameObject text1;
    private GameObject text2;
    private GameObject text3;
    private GameObject text4;

    private TMP_Text text1TMP;
    private TMP_Text text2TMP;
    private TMP_Text text3TMP;
    private TMP_Text text4TMP;

    void Start()
    {
        text1 = GameObject.FindGameObjectWithTag("Text1");
        text2 = GameObject.FindGameObjectWithTag("Text2");
        text3 = GameObject.FindGameObjectWithTag("Text3");
        text4 = GameObject.FindGameObjectWithTag("Text4");

        text1TMP = text1.GetComponent<TMP_Text>();
        text2TMP = text2.GetComponent<TMP_Text>();
        text3TMP = text3.GetComponent<TMP_Text>();
        text4TMP = text4.GetComponent<TMP_Text>();
    }

    public void StartFadeIn(string area)
    {
        
        StartCoroutine(FadeIn(area));
    }

    public void StartFadeOut(string area)
    {
        
        StartCoroutine(FadeOut(area));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator FadeIn(string area)
    {
        Debug.Log("FadeIn called");
        switch (area)
        {
            case "text1":
                yield return FadeText(text1TMP, 0f, 1f);
                break;

            case "text2":
                yield return FadeText(text2TMP, 0f, 1f);
                break;

            case "text3":
                yield return FadeText(text3TMP, 0f, 1f);
                break;

            case "text4":
                yield return FadeText(text4TMP, 0f, 1f);
                break;
        }
        StopCoroutine(FadeIn(area));
    }

    IEnumerator FadeOut(string area)
    {
        switch (area)
        {
            case "text1":
                yield return FadeText(text1TMP, 1f, 0f);
                break;

            case "text2":
                yield return FadeText(text2TMP, 1f, 0f);
                break;

            case "text3":
                yield return FadeText(text3TMP, 1f, 0f);
                break;

            case "text4":
                yield return FadeText(text4TMP, 1f, 0f);
                break;
        }
        StopCoroutine(FadeOut(area));
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
