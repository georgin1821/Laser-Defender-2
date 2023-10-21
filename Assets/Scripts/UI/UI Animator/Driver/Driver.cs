using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("_UIAnimation/Driver/Driver")]
public class Driver : MonoBehaviour
{
    public bool Autostart = false;
    public bool PrestartOnEnable = true;
    public bool ResetWhenFinish = false;
    public float duration;

    public DriverSettings Settings;

    [HideInInspector]
    public bool Running = false;
    public bool PlayForward = true;
    
    public IEnumerator routine;
    public float progress;

    public List<Controller> controllers = new List<Controller>();

    public DriverRemoteAccess remoteDriver = new DriverRemoteAccess();

    public void AutoFillControllers()
    {
        controllers = gameObject.GetComponents<Controller>().ToList();
    }

    private void OnValidate()
    {
        AutoFillControllers();
    }

    private void OnEnable()
    {
        remoteDriver.UpdateRemoteDriver(this);

        if (PrestartOnEnable == true) ResetProgress(PlayForward);

        if (Autostart == true) Run(PlayForward, progress);
    }

    public void ResetProgress(bool ToStart)
    {
        float result = 0f;

        if (ToStart == true)
        {
            result = 0;
            SetProgress(0);
        }
        else
        {
            result = 1;
            SetProgress(Settings.loop.loopCount * duration);
        }

        UpdateControllers(result);

    }

    public void SetProgress(float myProgress)
    {
        progress = myProgress;
    }

    public void RunForward()
    {
        Run(true, progress);
    }

    public void RunBackward()
    {
        Run(false, progress);
    }

    public void Run(bool _Forward, float _progress)
    {
        progress = _progress;
        PlayForward = _Forward;
        Stop();
        routine = DriverRoutine();
        StartCoroutine(routine);
    }

    public void Stop()
    {
        Running = false;
        if (routine != null) StopCoroutine(routine);
    }

    IEnumerator DriverRoutine()
    {
        Running = true;
        while (Running == true)
        {

            float WaitResult = Settings.wait.Wait;
            if (Settings.wait.Wait != 0 || Settings.wait.WaitMax != 0) WaitResult = Settings.wait.CalculateRandomWaitTime(Settings.wait.Wait, Settings.wait.WaitMax);
            if (WaitResult != 0) yield return new WaitForSecondsRealtime(WaitResult);

            float elapsedTime = 0f;
            float wholeDuration = Settings.loop.loopCount * duration;
            float result = 0f;
            bool EventToTrigger = false;

            if (PlayForward == false) elapsedTime = wholeDuration;

            elapsedTime = progress;

            while (0 <= elapsedTime && elapsedTime <= wholeDuration)
            {
                if (PlayForward == true) elapsedTime += Time.unscaledDeltaTime;
                else elapsedTime -= Time.deltaTime;

                progress = CalculateProgress(elapsedTime, wholeDuration);

                if (EventToTrigger == false)
                {
                    if (progress != 0 && progress != wholeDuration)
                    {
                        EventToTrigger = true;
                        Settings.events.StartEvent.Invoke();
                    }
                }

                if (Settings.loop.looptype == LoopType.repeat) result = Mathf.Repeat(progress / duration, 1.0001f);
                if (Settings.loop.looptype == LoopType.pingPong) result = Mathf.PingPong(progress / duration, 1.0001f);

                UpdateControllers(result);

                yield return null;
            }

            Running = Settings.loop.Autorepeat;
            if (Settings.loop.Autorepeat == true || ResetWhenFinish == true)
            {
                if (PlayForward == true) { progress = 0f; UpdateControllers(0); }
                else { progress = wholeDuration; UpdateControllers(1); } 
            }

            if (EventToTrigger == true) Settings.events.EndEvent.Invoke();
        }

        Running = false;
    }

    float CalculateProgress(float inTime, float duration)
    {
        if (inTime < 0) inTime = 0;
        if (inTime > duration) inTime = duration;

        return inTime;
    }

    public void UpdateControllers(float result)
    {
        foreach (var myController in controllers)
        {
            if (myController.gameObject.activeSelf == true) myController.setValues(result);
        }
    }
}