using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    [SerializeField] float introSpeed;

    float speed;
    float duration = 3f;
    bool isStatring = false;


    private void Awake()
    {
        GamePlayController.OnScrollingBGEnabled += OnScrollingBGEnabledHandler;
    }

    private void OnDestroy()
    {
        GamePlayController.OnScrollingBGEnabled -= OnScrollingBGEnabledHandler;
    }

    private void OnScrollingBGEnabledHandler()
    {
        IntroScrollingBG();
    }

    void Update()
    {

        if (transform.position.y < -14.66)
        {
            transform.transform.position = new Vector3(transform.position.x, 14.66f, transform.position.z);
        }
        if (isStatring) return;

        transform.Translate(Vector3.down * backgroundScrollSpeed * Time.deltaTime);
    }

    void IntroScrollingBG()
    {
        StopAllCoroutines();
        StartCoroutine(IntroScrollingRoutine());
    }
    IEnumerator IntroScrollingRoutine()
    {
        isStatring = true;
        
        float time = 0;
        while (time <= 3)
        {
           speed = Mathf.Lerp(introSpeed, backgroundScrollSpeed,  time / duration);
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        isStatring = false;
    }
}
