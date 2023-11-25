using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 0.5f;
    [SerializeField] private float introSpeed;
    [SerializeField] private float duration = 3f;
    // [SerializeField] int parallaxOrder; to iplement

    private float speed;
    private bool isStatring;
    private float distance;
    private Vector3 staringPos;
    private void Awake()
    {
        GamePlayController.OnScrollingBGEnabled += OnScrollingBGEnabledHandler;
        distance = GetComponentInChildren<SpriteRenderer>().size.y;
    }

    private void OnDestroy()
    {
        GamePlayController.OnScrollingBGEnabled -= OnScrollingBGEnabledHandler;
    }

    private void OnScrollingBGEnabledHandler()
    {
        IntroScrollingBG();
    }
    private void Start()
    {
        staringPos = new Vector3(0, 0, transform.position.z);
        transform.position = staringPos;

    }
    void Update()
    {
        if (transform.position.y < -distance)
        {
            transform.position = staringPos;
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
        while (time <= duration)
        {
            speed = Mathf.Lerp(introSpeed, backgroundScrollSpeed, time / duration);
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        isStatring = false;
    }
}
