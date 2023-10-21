using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isSpeedLevel;
    [HideInInspector] public LoadingFrom loadingFrom;

    public void LoadSceneWithNameFrom(string sceneName, LoadingFrom loadingFrom)
    {
        this.loadingFrom = loadingFrom;
        LoadingWithFadeScenes.Instance.LoadScene(sceneName);
    }
}

public enum LoadingFrom
{
    MAIN,
    LVLCOMP,
    DEFEAT
}

