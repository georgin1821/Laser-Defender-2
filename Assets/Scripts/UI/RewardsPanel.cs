using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class RewardsPanel : MonoBehaviour
{
    public static RewardsPanel instance;
   public bool[] dailyRewards;

    [SerializeField] List<Image> rewardsIcon;
    [SerializeField] List<Button> dailyRewardsButtons;
    [SerializeField] List<TMP_Text> rewardsText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        InitalizePanel();
    }
   public void InitalizePanel()
    {
        dailyRewards = GameDataManager.Instance.dailyRewards;

        for (int i = 0; i < dailyRewards.Length; i++)
        {
            if (dailyRewards[i])
            {
                rewardsIcon[i].gameObject.SetActive(true);
            }
            else
            {
                rewardsIcon[i].gameObject.SetActive(false);

                dailyRewardsButtons[i].interactable = false;
                rewardsText[i].gameObject.SetActive(false);
            }
        }

    }


}
