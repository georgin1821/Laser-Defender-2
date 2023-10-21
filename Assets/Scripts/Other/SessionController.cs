
using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class SessionController : MonoBehaviour
{
    public static SessionController instance;

    DateTime sessionTime = DateTime.UtcNow;
    DateTime currentTime;


    public bool[] rewardsChecked;
    public bool dailyRewardsReady = false;
    public event Action<int> OnDailyReawardReady;

    private void Awake()
    {
        Configure();
        rewardsChecked = new bool[30];
    }
    private void OnApplicationFocus(bool _focus)
    {
        if (_focus)
        {
            // Open a window to unpause the game
            // PageController.instance.TurnPageOn(PageType.PausePopup);
        }
        else
        {
            // Flag the game paused
            // m_IsPaused = true;
        }
    }

    private void Update()
    {
        currentTime = DateTime.UtcNow;
        RewardCheckOnStart(sessionTime, currentTime);
    }
    public void RewardCheckOnStart(DateTime sessionTimne, DateTime nextSessionTime)
    {
        int i = 0;
        if (nextSessionTime.Minute != sessionTimne.Minute)
        {
            dailyRewardsReady = true;
            rewardsChecked[i] = true;
          //  OnDailyReawardReady?.Invoke(i);
            Debug.Log("Reward");
            GameDataManager.Instance.dailyRewards[i] = true;
            RewardsPanel.instance.InitalizePanel();
            i++;
            sessionTime = DateTime.UtcNow;
        }

    }

    public void UnPause()
    {
        // m_IsPaused = false;
    }
    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}





