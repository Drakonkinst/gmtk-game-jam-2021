using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothUIBar : MonoBehaviour
{
    public float smoothTime = 0.3f;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.5f;

    private Transform myTransform;
    private RectTransform bar;
    private float barWidth;

    private float currentPercent = 1.0f;
    private float targetPercent = 1.0f;
    private float currentVelocity = 0.0f;
    private Vector3 originalPosition;
    private float shakeDurationRemaining = 0.0f;


    private void Awake()
    {
        myTransform = transform.parent.transform;
        bar = GetComponent<RectTransform>();
        barWidth = transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        originalPosition = myTransform.position;
    }

    private void Update()
    {
        SetActualPercent(Mathf.SmoothDamp(currentPercent, targetPercent, ref currentVelocity, smoothTime));

        if(shakeDurationRemaining > 0)
        {
            myTransform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            shakeDurationRemaining -= Time.deltaTime;
        } else
        {
            shakeDurationRemaining = 0.0f;
        }
    }

    public void SetPercent(float percent)
    {
        targetPercent = Mathf.Clamp01(percent);
    }

    // Try not to use this unless explicitly setting
    public void SetActualPercent(float percent)
    {
        currentPercent = Mathf.Clamp01(percent);
        bar.sizeDelta = new Vector2(currentPercent * barWidth, bar.sizeDelta.y);
    }

    public void Shake()
    {
        shakeDurationRemaining = shakeDuration;
    }
}
