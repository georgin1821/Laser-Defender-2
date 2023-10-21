using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingWithFadeScenes : Singleton<LoadingWithFadeScenes>
{

    public GameObject loadingScreen;
    string sceneToLoad;
    public CanvasGroup canvasGroup;
    IEnumerator StartLoad()
    {
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
    }
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }

    public void LoadScene(String name)
    {
        sceneToLoad = name;
        StartCoroutine(StartLoad());
    }
}

