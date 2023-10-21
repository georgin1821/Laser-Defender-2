using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MainSceneMenuController : MonoBehaviour
{
    public static MainSceneMenuController instance;

    public float amountUpgrade = 100;

    [SerializeField] GameObject profilePanel, shopPanel, squapPanel, rewardsPanel, settingsPanel, questPanel;
    [SerializeField] Button shipBtn, upgradeBtn;
    [SerializeField] Sprite[] sprites;
    [SerializeField] TMP_Text powerText, upgradeInfo, coinsText;
    int selectedShip;
    public string panelName;
    string shipName;
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
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();

        squapPanel.SetActive(true);

    }

    public void LoadLevelScene()
    {
        AudioController.Instance.PlayAudio(AudioType.UI_click_magic_2);
        GameManager.Instance.loadingFrom = LoadingFrom.MAIN;
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
    }

    public void SelectShip()
    {
        shipName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (shipName)
        {
            case "Ship1":
                selectedShip = 0;
                UpdateUpgradeInfo();

                break;

            case "Ship2":
                selectedShip = 1;

                UpdateUpgradeInfo();

                break;

        }

    }

    public void UpgradeShip()
    {
        {
            switch (selectedShip)
            {
                case 0:

                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }

                    break;
                case 1:

                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }
                    break;
            }
        }
    }

    private void ClosePanels()
    {
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
        squapPanel.SetActive(false);
        settingsPanel.SetActive(false);
        questPanel.SetActive(false);
    }
    private void Upgrade()
    {

        GameDataManager.Instance.selectedShip = selectedShip;
        int power = GameDataManager.Instance.shipsPower[selectedShip] + 100;
        GameDataManager.Instance.shipsPower[selectedShip] = power;
        GameDataManager.Instance.coins -= (int)amountUpgrade;
        GameDataManager.Instance.shipsRank[selectedShip]++;
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        upgradeInfo.text = amountUpgrade.ToString();

        coinsText.text = GameDataManager.Instance.coins.ToString();

        GameDataManager.Instance.Save();
    }
    private void UpdateUpgradeInfo()
    {
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        shipBtn.image.sprite = sprites[selectedShip];

        upgradeInfo.text = amountUpgrade.ToString();
    }
}
