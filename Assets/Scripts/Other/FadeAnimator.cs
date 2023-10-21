using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeAnimator : MonoBehaviour
{
    public static event Action OnEventFadeInComplete;
    public static event Action OnEventFadeOutComplete;

    public void OnFadeInComplete()
    {
        OnEventFadeInComplete?.Invoke();
    }

    public void OnFadeOutComplete()
    {
        OnEventFadeOutComplete?.Invoke();
    }

}
