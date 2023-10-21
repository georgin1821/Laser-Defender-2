using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimType
{
    Position,
    Rotation,
    Scale,
    RectPosition
}

[AddComponentMenu("_UIAnimation/Controller/Controller Transform")]
public class ControllerTransform : Controller
{
    public AnimType animType;
    public Transform myTransform;

    public Vector3 inAxes = Vector3.one;

    public Vector3 ResultInAxes(Vector3 originalVector, float FloatResult, Vector3 inAxes)
    {
        float X = originalVector.x;
        float Y = originalVector.y;
        float Z = originalVector.z;

        if (inAxes.x != 0) X = FloatResult * inAxes.x;
        if (inAxes.y != 0) Y = FloatResult * inAxes.y;
        if (inAxes.z != 0) Z = FloatResult * inAxes.z;

        return new Vector3(X, Y, Z);
    }

    private Vector3 GetvaluesInAxes(float myFloat)
    {
        return ResultInAxes(GetTransform(), myFloat, inAxes);
    }

    private Vector3 GetTransform()
    {
        switch (animType)
        {
            case AnimType.Position: return myTransform.localPosition;
            case AnimType.Rotation:return myTransform.localRotation.eulerAngles;
            case AnimType.Scale: return myTransform.localScale;
            case AnimType.RectPosition: return ((RectTransform)myTransform).anchoredPosition3D;
            default: return Vector3.zero;
        }
    }

    private void SetTransform(Vector3 resulVector)
    {
        switch (animType)
        {
            case AnimType.Position: myTransform.localPosition = resulVector;break;
            case AnimType.Rotation: myTransform.localRotation = Quaternion.Euler(resulVector);break;
            case AnimType.Scale:myTransform.localScale = resulVector;break;
            case AnimType.RectPosition: ((RectTransform)myTransform).anchoredPosition3D = resulVector;break;
        }
    }

    public override void SetValuesFromCurve(float curveResult)
    {
        Vector3 resultVector = GetvaluesInAxes(curveResult);
        SetTransform(resultVector);
    }

    public override void GetInitialValue(Controller_EditorPreview editorPreview)
    {
        editorPreview.initialVector3 = GetTransform();

        editorPreview.initialString = editorPreview.initialVector3.ToString();
    }

    public override void SetInitialValue(Controller_EditorPreview editorPreview)
    {
        SetTransform(editorPreview.initialVector3);
    }
}
