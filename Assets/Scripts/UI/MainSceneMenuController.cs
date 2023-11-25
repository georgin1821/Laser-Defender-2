using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MainSceneMenuController : MonoBehaviour
{
    public static MainSceneMenuController instance;


    [SerializeField] GameObject profilePanel, shopPanel, squapPanel, rewardsPanel, settingsPanel, questPanel;
    [SerializeField] Button shipBtn, upgradeBtn;
    [SerializeField] TMP_Text  coinsText;
    public string panelName;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        SessionController.instance.OnDailyReawardReady -= OnDailyRewardsReadyHandlere;
    }
    private void OnDailyRewardsReadyHandlere(int obj)
    {
        rewardsPanel.SetActive(true);
    }

    private void Start()
    {
        SessionController.instance.OnDailyReawardReady += OnDailyRewardsReadyHandlere;
    }

    public void ActivatePanels()
    {
        panelName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        switch (panelName)
        {
            case "Shop":
                shopPanel.SetActive(!shopPanel.activeInHierarchy);
                profilePanel.SetActive(false);
                settingsPanel.SetActive(false);
                questPanel.SetActive(false);
                break;
            case "Profile":
                profilePanel.SetActive(!profilePanel.activeInHierarchy);
                shopPanel.SetActive(false);
                settingsPanel.SetActive(false);
                questPanel.SetActive(false);
                break;
            case "Settings":
                settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
                profilePanel.SetActive(false);
                shopPanel.SetActive(false);
                questPanel.SetActive(false);
                break;
            case "Quest":
                questPanel.SetActive(!questPanel.activeInHierarchy);
                settingsPanel.SetActive(false);
                profilePanel.SetActive(false);
                shopPanel.SetActive(false);
                break;
            case "Home":
                ClosePanels();
                break;
        }
        AudioController.Instance.PlayAudio(AudioType.UI_click_switch);
    }

    public void ShowHomeScreen()
    {
        AudioController.Instance.PlayAudio(AudioType.UI_click_magic);
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
        squapPanel.SetActive(false);
        settingsPanel.SetActive(false);
        questPanel.SetActive(false);
    }

    public void ShowSquadPanel()
    {

        squapPanel.SetActive(true);

    }

    public void LoadLevelScene()
    {
        AudioController.Instance.PlayAudio(AudioType.UI_click_magic_3);
        GameManager.Instance.loadingFrom = LoadingFrom.MAIN;
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
    }



    private void ClosePanels()
    {
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
        squapPanel.SetActive(false);
        settingsPanel.SetActive(false);
        questPanel.SetActive(false);
    }
}
