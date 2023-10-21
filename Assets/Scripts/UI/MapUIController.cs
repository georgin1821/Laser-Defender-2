using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;

    [SerializeField] GameObject[] levelStarGameObjects;
    [SerializeField] Sprite[] starImages = new Sprite[4];
    [SerializeField] Sprite planetImageLocked;
    [SerializeField] Sprite planetImageUnLocked;
    [SerializeField] List<Button> levelButtons;

    [SerializeField] TMP_Text lvlCompleteInfo, scoreTxt, coinsRewardTxt, levelStartTxt;
    [SerializeField] GameObject defeatPanel, lvlStartPanel, lvlCompletePanel;

    LoadingFrom load;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitializeLevelMenu();
    }
    private void Start()
    {
        load = GameManager.Instance.loadingFrom;
        switch (load)
        {
            case LoadingFrom.MAIN:
                lvlCompletePanel.SetActive(false);
                defeatPanel.SetActive(false);
                lvlStartPanel.SetActive(false);
                break;
            case LoadingFrom.LVLCOMP:
                lvlCompletePanel.SetActive(true);
                defeatPanel.SetActive(false);
                lvlStartPanel.SetActive(false);
                break;
            case LoadingFrom.DEFEAT:
                lvlCompletePanel.SetActive(false);
                defeatPanel.SetActive(true);
                lvlStartPanel.SetActive(false);
                break;
        }
    }

    void InitializeLevelMenu()
    {
        bool[] levels = GameDataManager.Instance.levels;

        for (int i = 0; i < levels.Length; i++)
        {
            levelStarGameObjects[i].transform.GetChild(0).gameObject.SetActive(false);
            levelStarGameObjects[i].transform.GetChild(1).gameObject.SetActive(false);
            levelStarGameObjects[i].transform.GetChild(2).gameObject.SetActive(false);
            levelStarGameObjects[i].transform.GetChild(3).gameObject.SetActive(false);

            if (levels[i])
            {
                levelButtons[i].interactable = true;
                levelButtons[i].image.sprite = planetImageUnLocked;
            }
            else
            {
                levelButtons[i].interactable = false;
            }

            LevelCompletedDifficulty levelCompletedDifficulty = GameDataManager.Instance.levelCompletedDifficulty[i];

            switch (levelCompletedDifficulty)
            {
                case LevelCompletedDifficulty.NONE:
                    levelStarGameObjects[i].transform.GetChild(3).gameObject.SetActive(true);
                    levelStarGameObjects[i].transform.GetChild(3).GetComponent<Image>().sprite = starImages[0];
                    break;

                case LevelCompletedDifficulty.EASY:
                    levelStarGameObjects[i].transform.GetChild(3).gameObject.SetActive(true);
                    levelStarGameObjects[i].transform.GetChild(3).GetComponent<Image>().sprite = starImages[1];
                    break;

                case LevelCompletedDifficulty.MEDIUM:
                    levelStarGameObjects[i].transform.GetChild(2).gameObject.SetActive(true);
                    levelStarGameObjects[i].transform.GetChild(2).GetComponent<Image>().sprite = starImages[2];
                    break;

                case LevelCompletedDifficulty.HARD:
                    levelStarGameObjects[i].transform.GetChild(1).gameObject.SetActive(true);
                    levelStarGameObjects[i].transform.GetChild(1).GetComponent<Image>().sprite = starImages[3];
                    break;
            }
        }
    }
    public void StageCompleteScreen()
    {
        lvlCompleteInfo.text = "Stage " + GameDataManager.Instance.LevelIndex + 1 + " " + GameDataManager.Instance.currentDifficulty;
        scoreTxt.text = GameDataManager.Instance.LevelScore.ToString();
        coinsRewardTxt.text = GameDataManager.Instance.LevelCoins.ToString();
    }
    public void SelectLevel()
    {
        string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (level)
        {
            case "Level0":
                GameDataManager.Instance.CurrentLevel = 0;
                break;
            case "Level1":
                GameDataManager.Instance.CurrentLevel = 1;
                break;
            case "Level2":
                GameDataManager.Instance.CurrentLevel = 2;
                break;
            case "Level3":
                GameDataManager.Instance.CurrentLevel = 3;
                break;
            case "Level4":
                GameDataManager.Instance.CurrentLevel = 4;
                break;
        }

        StartLevelPanelInfo();
    }
    public void LoadLevel()
    {
        LoadingWithFadeScenes.Instance.LoadScene("Game");
    }
    public void HidePanels()
    {
        defeatPanel.SetActive(false);
        lvlCompletePanel.SetActive(false);
    }
    public void NextLevel()
    {
        StartLevelPanelInfo();
        lvlCompletePanel.SetActive(false);
        defeatPanel.SetActive(false);
        GameDataManager.Instance.CurrentLevel++;
    }
    public void BackToMainScene()
    {
        LoadingWithFadeScenes.Instance.LoadScene("MainScene");

    }
    public void StartLevelPanelInfo()
    {
        HidePanels();
        lvlStartPanel.SetActive(true);
        levelStartTxt.text = GameDataManager.Instance.CurrentLevel.ToString();

    }
}
