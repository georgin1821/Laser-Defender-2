using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public List<Quest> currentQuestList = new List<Quest>();
    private void Awake()
    {
        WaveSpawner.OnEnemiesDieCount += OnEnemiesDieCountHandler;
    }
    private void OnDestroy()
    {
        WaveSpawner.OnEnemiesDieCount -= OnEnemiesDieCountHandler;
    }
    void Start()
    {
        currentQuestList = QuestManager.Instance.currentQuestList;
    }
    private void OnEnemiesDieCountHandler(int enemiesCount)
    {
        string qe = "Kill 1000 enemies";
        QuestManager.Instance.AddQuestItem(qe, enemiesCount);
    }

    public void CheckCompletedQuest()
    {
        for (int i = 0; i < currentQuestList.Count; i++)
        {
            if (currentQuestList[i].progress == Quest.QuestProgress.COMPLETE)
            {

            }
        }
    }
}
