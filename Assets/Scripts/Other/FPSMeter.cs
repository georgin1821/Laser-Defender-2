using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSMeter : MonoBehaviour
{
    [SerializeField] TMP_Text frameRateText;
    [SerializeField] TMP_Text maxFrameRateText;
    [SerializeField] public TMP_Text minFrameRateText;

    float frameCounter = 0;
    float refreshRate = 0.1f;
    float timeCounter;

    float maxFrameRate = 0f;
    float minFrameRate = 1000f;

    void Start()
    {
        StartCoroutine(ResetMinFramerate());
    }

    IEnumerator ResetMinFramerate()
    {
        yield return new WaitForSeconds(1f);
        minFrameRate = 1000f;
    }
    void Update()
    {

        if (timeCounter < refreshRate)
        {
            frameCounter++;
            timeCounter += Time.deltaTime;
        }
        else
        {
            int lastFramRate =Mathf.RoundToInt( frameCounter / timeCounter);
            if (minFrameRate > lastFramRate) minFrameRate = lastFramRate;
            if (maxFrameRate < lastFramRate) maxFrameRate = lastFramRate;

            frameRateText.text = lastFramRate.ToString();
            minFrameRateText.text = minFrameRate.ToString();
            maxFrameRateText.text = maxFrameRate.ToString();

            frameCounter = 0;
            timeCounter = 0;
        }
    }
}
