using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> questList = new List<Quest>(); //Master Quest List
    public List<Quest> currentQuestList = new List<Quest>();

    protected override void Awake()
    {
        base.Awake();
        InitializeCurrentQuestList();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public bool RequestAvailableQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE)
            {
                return true;
            }
        }
        return false;
    }
    public bool RequestQompleteQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.COMPLETE)
            {
                return true;
            }
        }
        return false;
    }

    //ADD ITEMS
    public void AddQuestItem(string questObjective, int itemAmount)
    {
        for (int i = 0; i < currentQuestList.Count; i++)
        {
            if (currentQuestList[i].questObjective == questObjective)
            {
                currentQuestList[i].questObjectiveCount += itemAmount;
            }
            if (currentQuestList[i].questObjectiveCount >= currentQuestList[i].questObjectiveRequirement)
            {
                currentQuestList[i].progress = Quest.QuestProgress.COMPLETE;
            }
        }
    }
    public void CompleteQuest(int questID)
    {
        for (int i = 0; i < currentQuestList.Count; i++)
        {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.COMPLETE)
            {
                currentQuestList[i].progress = Quest.QuestProgress.DONE;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
        CheckChainQuest(questID);
    }
    private void CheckChainQuest(int questID)
    {
        int tempID = 0;
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID)
            {
                tempID = questList[i].nextQuest;
            }
        }
        if (tempID > 0)
        {
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].id == tempID && questList[i].progress == Quest.QuestProgress.NOT_AVAILABLE)
                {
                    questList[i].progress = Quest.QuestProgress.AVAILABLE;
                }
            }
        }
    }

    public void InitializeCurrentQuestList()
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].progress == Quest.QuestProgress.AVAILABLE)
            {
                currentQuestList[i] = questList[i];
            }
        }
    }
    public void QusetRequest(QuestObject questObject)
    {
    }
}
