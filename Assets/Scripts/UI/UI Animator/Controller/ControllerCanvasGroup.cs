using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("_UIAnimation/Controller/Controller Canvas Group")]
public class ControllerCanvasGroup : Controller
{
    public CanvasGroup myCanvasGroup;

    public override void GetInitialValue(Controller_EditorPreview editorPreview)
    {
        editorPreview.initialFloat = myCanvasGroup.alpha;

        editorPreview.initialString = editorPreview.initialFloat.ToString();
    }

    public override void SetInitialValue(Controller_EditorPreview editorPreview)
    {
        SetCanvasGroupAlpha(editorPreview.initialFloat);
    }

    public override void SetValuesFromCurve(float curveResult)
    {
        SetCanvasGroupAlpha(curveResult);
    }

    private void SetCanvasGroupAlpha(float myFloat)
    {
        myCanvasGroup.alpha = myFloat;
    }

}
