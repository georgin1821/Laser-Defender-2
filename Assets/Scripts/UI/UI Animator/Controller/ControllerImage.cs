using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnimImageType
{
    FillImage,
    ImageFade
}

[AddComponentMenu("_UIAnimation/Controller/Controller Image")]
public class ControllerImage : Controller
{

    public AnimImageType animType;
    public Image myImage;

    public override void SetValuesFromCurve(float curveResult)
    {
        switch (animType)
        {
            case AnimImageType.FillImage: SetImageFill(curveResult);break;
            case AnimImageType.ImageFade: SetImageFade(curveResult);break;
        }
    }

    public void SetImageFill(float myFloat)
    {
        myImage.fillAmount = myFloat;
    }

    public void SetImageFade(float myFloat)
    {
        Color newColor = myImage.color;
        newColor.a = myFloat;
        myImage.color = newColor;
    }

    public override void GetInitialValue(Controller_EditorPreview editorPreview)
    {
        switch (animType)
        {
            case AnimImageType.FillImage: editorPreview.initialFloat = myImage.fillAmount; break;
            case AnimImageType.ImageFade: editorPreview.initialFloat = myImage.color.a;   break;
        }

        editorPreview.initialString = editorPreview.initialFloat.ToString();
    }

    public override void SetInitialValue(Controller_EditorPreview editorPreview)
    {
        SetValuesFromCurve(editorPreview.initialFloat);
    }
}
