using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DriverSettings
{
    public LoopSettings loop;
    public WaitSettings wait;
    public EventSettings events;
}

public enum LoopType
{
    repeat,
    pingPong
}

[System.Serializable]
public class LoopSettings
{
    public bool Autorepeat = false;
    public int loopCount = 1;
    public LoopType looptype;
}

[System.Serializable]
public class WaitSettings
{
    public float Wait;
    public float WaitMax;

    public float CalculateRandomWaitTime(float StartWaitTime, float MaxWaitTime)
    {
        if (MaxWaitTime > StartWaitTime) return Random.RandomRange(StartWaitTime, MaxWaitTime);
        else return StartWaitTime;
    }
}

[System.Serializable]
public class EventSettings
{
    public UnityEvent StartEvent;
    public UnityEvent EndEvent;
}
