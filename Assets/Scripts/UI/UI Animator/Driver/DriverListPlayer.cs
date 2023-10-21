using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaitType
{
    RunAllAtOnce,
    WaitForPreviousToFinish
}

[AddComponentMenu("_UIAnimation/Driver/Driver List Player")]
public class DriverListPlayer : MonoBehaviour
{
    public bool PlayForward = true;
    public List<Driver> AnimList;
    public WaitType waitType;
    public float WaitInBetween;
    public bool Autostart = false;
    public WaitSettings wait;
    public EventSettings events;

    IEnumerator myCoroutine;

    private void Start()
    {
        if (Autostart == true) PlayListInDirection();
    }

    public void PlayListInDirection()
    {
        RunList(PlayForward);
    }

    public void RunList(bool Forward)
    {
        if (myCoroutine != null) StopCoroutine(myCoroutine);
        myCoroutine = ListRunner(Forward);
        StartCoroutine(myCoroutine);
    }

    public void ResetProgress(bool ToStart)
    {
        foreach (var myDriver in AnimList)
        {
            myDriver.ResetProgress(ToStart);
        }
    }

    public IEnumerator ListRunner(bool Forward)
    {
        events.StartEvent.Invoke();

        float WaitResult = wait.Wait;
        if (wait.Wait != 0 || wait.WaitMax != 0) WaitResult = wait.CalculateRandomWaitTime(wait.Wait, wait.WaitMax);
        if (WaitResult != 0) yield return new WaitForSeconds(WaitResult);

        foreach (var myDriver in AnimList)
        {
            if (waitType == WaitType.RunAllAtOnce)
            {
                myDriver.Run(Forward, myDriver.progress);
            }

            if (waitType == WaitType.WaitForPreviousToFinish)
            {
                myDriver.Run(Forward, myDriver.progress);
                yield return new CustomYieldDriverFinished(myDriver);
            }

            if (WaitInBetween != 0) yield return new WaitForSeconds(WaitInBetween);
        }

        yield return new CustomYieldDriverListFinished(AnimList);

        events.EndEvent.Invoke();
    }
}
