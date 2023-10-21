using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Controller_EditorPreview editorPreview = new Controller_EditorPreview();
    public abstract void GetInitialValue(Controller_EditorPreview editorPreview);
    public abstract void SetInitialValue(Controller_EditorPreview editorPreview);

    public AnimationCurve animCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float from = 0;
    public float to = 1;

    public Driver DrivenBy;
    public int RemoteDriverID;
    public float inputOffset;

    public abstract void SetValuesFromCurve(float curveResult);

    private void OnEnable()
    {
        ConnectRemoteDriver();
    }

    private void OnDisable()
    {
        DrivenBy.controllers.Remove(this);
    }

    public void ConnectRemoteDriver()
    {
        if (RemoteDriverID != 0)
        {
            if (DriverRemoteAccess.RemoteDriverDict.ContainsKey(RemoteDriverID))
            {
                DrivenBy = DriverRemoteAccess.RemoteDriverDict[RemoteDriverID];
                DrivenBy.controllers.Add(this);
            }
        }
    }

    private void Start()
    {
        ConnectRemoteDriver();
        editorPreview.preview = false;
        editorPreview.PreviewMyValue(this);
    }

    private void OnValidate()
    {
        AutoFillDriver();
        editorPreview.PreviewMyValue(this);
    }

    public void AutoFillDriver()
    {
        DrivenBy = gameObject.GetComponent<Driver>();
        if (DrivenBy != null) DrivenBy.AutoFillControllers();
    }

    public float ValueFromCurve(float myfloat)
    {
        float offset = 0;
        if (inputOffset != 0) offset = inputOffset / 100;
        float Result = Mathf.Repeat((myfloat + offset), 1.00001f);

        float CurveResult = animCurve.Evaluate(Result);
        return Mathf.LerpUnclamped(from, to, CurveResult);
    }

    public void setValues(float result)
    {
        float Remap = ValueFromCurve(result);
        SetValuesFromCurve(Remap);
    }
}
