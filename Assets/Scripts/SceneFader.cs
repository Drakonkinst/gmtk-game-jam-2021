using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    private const float updateTick = 0.01f;

    public static SceneFader instance;

    public float fadeSpeed = 2.0f;

    private Image screen;

    private void Awake()
    {
        instance = this;
        screen = GetComponent<Image>();
    }

    void Start()
    {
        SetOpacity(1.0f);
        FadeIn();
    }

    private void SetOpacity(float percent)
    {
        Color color = screen.color;
        color.a = percent;
        screen.color = color;
    }

    public void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    private IEnumerator DoFadeIn()
    {
        float timeLeft = fadeSpeed;
        while(timeLeft > 0)
        {
            yield return new WaitForSeconds(updateTick);
            timeLeft -= Mathf.Max(0.0f, updateTick);
            SetOpacity(timeLeft / fadeSpeed);
        }
    }

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }    

    private IEnumerator DoFadeOut()
    {
        float timeLeft = fadeSpeed;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(updateTick);
            timeLeft -= Mathf.Max(0.0f, updateTick);
            SetOpacity(1 - timeLeft / fadeSpeed);
        }
    }
}
