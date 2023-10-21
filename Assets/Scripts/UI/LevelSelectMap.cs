using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelSelectMap : MonoBehaviour
{
    [SerializeField] GameObject easy, medium, hard;


    private GameDataManager GameData;
    private Button easyBtn, mediumBtn, hardButton;
    private Animator easyAnim, mediumAnim, hardAnim;
    private string difficulty;

    private void Start()
    {
        //access buttons and anims components
        AccessButtonsAndAnimComponents();
        InitializeLevelMenu();
    }

    private void InitializeLevelMenu()
    {
        //cashing GameDataManager and currentSelectedLevel
        int currentLevel = GameData.CurrentLevel;
        LevelCompletedDifficulty completedDifficulty = GameData.levelCompletedDifficulty[currentLevel];

        //first disable all buttons and buttons animations
        DisableAllDifficultyButtons();

        /* enables the difficulty button and all previous of the difficulty the player
        has finished the current level but enables the last  button anim 
        also set the current difficulty*/
        switch (completedDifficulty)
        {
            case LevelCompletedDifficulty.NONE:
                easyAnim.enabled = true;
                easyBtn.interactable = true;
                GameDataManager.Instance.currentDifficulty = GameDifficulty.EASY;
                break;
            case LevelCompletedDifficulty.EASY:
                easyAnim.enabled = true;
                easyBtn.interactable = true;
                GameDataManager.Instance.currentDifficulty = GameDifficulty.EASY;
                break;
            case LevelCompletedDifficulty.MEDIUM:
                mediumAnim.enabled = true;
                easyBtn.interactable = true;
                mediumBtn.interactable = true;
                GameDataManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
                break;
            case LevelCompletedDifficulty.HARD:
                hardAnim.enabled = true;
                easyBtn.interactable = true;
                mediumBtn.interactable = true;
                hardButton.interactable = true;
                GameDataManager.Instance.currentDifficulty = GameDifficulty.HARD;
                break;
        }
    }

    /* Select a button difficulty enables animation of it and disable all others
    and set the difficulty of level */
    public void SelectDifficulty()
    {
        difficulty = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        switch (difficulty)
        {
            case "Easy":
                GameDataManager.Instance.currentDifficulty = GameDifficulty.EASY;
                easyAnim.enabled = true;
                hardAnim.enabled = false;
                hard.GetComponentInParent<Animator>().enabled = false;
                break;
            case "Medium":
                GameDataManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
                easyAnim.enabled = false;
                mediumAnim.enabled = true;
                hardAnim.enabled = false;
                break;
            case "Hard":
                GameDataManager.Instance.currentDifficulty = GameDifficulty.HARD;
                easyAnim.enabled = false;
                mediumAnim.enabled = false;
                hardAnim.enabled = true;
                break;
        }
    }

    // Invokes when pressing the back button and returns to Map panel
    public void BackToMap()
    {
        gameObject.SetActive(false);
    }
    private void DisableAllDifficultyButtons()
    {
        easyAnim.enabled = false;
        mediumAnim.enabled = false;
        hardAnim.enabled = false;
        easyBtn.interactable = false;
        mediumBtn.interactable = false;
        hardButton.interactable = false;
    }
    private void AccessButtonsAndAnimComponents()
    {
        GameData = GameDataManager.Instance;
        easyBtn = easy.GetComponentInChildren<Button>();
        mediumBtn = medium.GetComponentInChildren<Button>();
        hardButton = hard.GetComponentInChildren<Button>();
        easyAnim = easy.GetComponent<Animator>();
        mediumAnim = medium.GetComponent<Animator>();
        hardAnim = hard.GetComponent<Animator>();
    }
}
