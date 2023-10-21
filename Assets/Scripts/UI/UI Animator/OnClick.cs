using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OnClick : MonoBehaviour
{
    public Button myButton;
    public UnityEvent ButtonClick;


    private void OnValidate()
    {
        if (myButton != null)
        {
            ButtonClick = myButton.onClick;
        }
    }
}
