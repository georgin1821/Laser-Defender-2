using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public enum QuestProgress
    {
        NOT_AVAILABLE,
        AVAILABLE,
        COMPLETE,
        DONE
    }

    public QuestProgress progress; // state of current quest

    public string title;
    public string itemReward;
    public string questObjective;

    public int id;
    public int questObjectiveCount;
    public int questObjectiveRequirement;
    public int nextQuest;
    public int goldReward;

}
