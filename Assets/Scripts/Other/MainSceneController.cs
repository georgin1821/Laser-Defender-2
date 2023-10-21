using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class MainSceneController : MonoBehaviour
{
    public static MainSceneController instance;

    [SerializeField] AudioType soundtrack;
    [SerializeField] TMP_Text timeText;

    private DateTime localTime;

    private void Awake()
    {
        Configure();
    }
    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayAudio(soundtrack, true, 2);
        StartCoroutine(UpdateTimePanel());
    }

    private void OnDisable()
    {
        AudioController.Instance.StopAudio(soundtrack, true);
    }

    IEnumerator UpdateTimePanel()
    {
        while (true)
        {
            localTime = DateTime.Now;
            string time = localTime.ToString("d/M H:mm");
            timeText.text = time;
            yield return new WaitForSeconds(60);
        }
    }

}

