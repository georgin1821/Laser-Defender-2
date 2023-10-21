using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controller_EditorPreview
{
    public bool preview = false;

    [Range(0,1)]
    public float previewResult;
    [HideInInspector]
    public bool iHaveInitialValue = false;
    [HideInInspector]
    public float initialFloat;
    [HideInInspector]
    public Vector3 initialVector3;

    public string initialString;

    public void PreviewMyValue(Controller myController)
    {
        if (preview == true)
        {
            if (iHaveInitialValue == false)
            {
                myController.GetInitialValue(this);
                iHaveInitialValue = true;
            }

            myController.setValues(previewResult);
        }
        else
        {
            if (iHaveInitialValue == true)
            {
                myController.SetInitialValue(this);
                iHaveInitialValue = false;
            }
        }
    }
}
