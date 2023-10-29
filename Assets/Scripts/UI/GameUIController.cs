using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : SimpleSingleton<GameUIController>
{
    [SerializeField] private TMP_Text scoreText, lostTxt;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text waveText, victoryTxt;
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text gunRankText;
    [SerializeField] private Button skill1Btn;
    [SerializeField] private Button continueBtn, continue2Btn;
    [SerializeField] private Image semiTransperantImage;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private Animator anim;

    [Header("Sound")]
    [SerializeField] private AudioClip click1;

    [SerializeField] private AudioClip click2;

    public Animator anim1;
    public AnimationClip clip;

    #region Unity Functions

   override protected void Awake()
    {
        base.Awake();        //register event at GPC ao game state changes
        GamePlayController.OnGameStateChange += OnGameStateChangeMenuActivation;

        continueBtn.onClick.AddListener(ContinueGameRoutine);
    }

    private void Start()
    {
        coinsText.text = GameDataManager.Instance.coins.ToString();
        levelText.text = GameDataManager.Instance.CurrentLevel.ToString();
    }


    #endregion Unity Functions

    override protected void OnDestroy()
    {
        base.OnDestroy();
        GamePlayController.OnGameStateChange -= OnGameStateChangeMenuActivation;
        continueBtn.onClick.RemoveListener(ContinueGameRoutine);
    }

    private void OnApplicationPause(bool pause)
    {
       // Debug.Log("pause");
    }

    private void OnGameStateChangeMenuActivation(GameState state)
    {
        defeatPanel.SetActive(state == GameState.DEFEAT);
        semiTransperantImage.gameObject.SetActive(state == GameState.DEFEAT);
        victoryTxt.gameObject.SetActive(state == GameState.LEVELCOMPLETE);
        switch (state)
        {
            case GameState.DEFEAT:
                if (GameDataManager.Instance.gems < 10)
                {
                    continue2Btn.interactable = false;
                }

                break;
        }
    }


    public void SpendGemsToRevive()
    {
        if (GameDataManager.Instance.gems >= 10)
        {
            GameDataManager.Instance.gems -= 10;
            ContinueGameRoutine();
        }
    }
    public void ExitToDefeatPanel()
    {
        StartCoroutine(AfterDefeatRoutine());
    }
    public void ResumeGame()
    {
        pausePanel.GetComponent<CoolDownCounter>().StartCountDown();
    }

    public void BackToMap()
    {
        Time.timeScale = 1;
        GamePlayController.Instance.UpdateState(GameState.EXIT);
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "" + score;
    }

    public void ShowWaveInfoText(int waveIndex, int wavesTotal, string name = "")
    {
        string text = "";
        if (waveIndex == 0)
        {
            introText.gameObject.SetActive(true);
        }
        if (name != "")
        {
            text = name;
            introText.gameObject.SetActive(true);
            introText.text = "!!BOSS APPROCHING!!";
        }
        else
        {
            text = "WAVE " + (waveIndex + 1) + "/" + wavesTotal;
        }

        waveText.gameObject.SetActive(true);
        waveText.text = text;
        Invoke("WaveTextDisable", 4);
    }

    private void WaveTextDisable()
    {
        waveText.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);
    }

    public void UpdateWeaponRankStatus(int ranks)
    {
        if (ranks < 9)
        {
            gunRankText.text = ranks.ToString();
        }
        else
        {
            gunRankText.text = "MAX";
        }
    }

    public void OpenPausePanel()
    {
        GamePlayController.Instance.UpdateState(GameState.PAUSE);
        pausePanel.SetActive(true);
    }


    public void EnemyHealthSliderConfigure(int health)
    {
        enemyHealthSlider.gameObject.SetActive(true);
        enemyHealthSlider.maxValue = health;
        enemyHealthSlider.minValue = 0;
        enemyHealthSlider.value = health;
    }

    public void EnemyHealthSliderUpdate(int value)
    {
        enemyHealthSlider.value = value;
    }

    private void ContinueGameRoutine()
    {
        defeatPanel.SetActive(false);
        semiTransperantImage.gameObject.SetActive(false);
        GamePlayController.Instance.BeginIntroSequence(true);

        GamePlayController.Instance.UpdateState(GameState.PLAY);
        Player.Instance.ShieldsUp();
    }

    public IEnumerator UpdateScoreRoutine(int CurrentScore, int newScore)
    {
        while (CurrentScore < newScore)
        {
            CurrentScore = (int)Mathf.MoveTowards(CurrentScore, newScore, 1f);
            scoreText.text = CurrentScore.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public IEnumerator UpdateCoinsRoutine(int coins, int coinsToAdd)
    {
        while (coins < coinsToAdd)
        {
            coins = (int)Mathf.MoveTowards(coins, coinsToAdd, 4f);
            coinsText.text = coins.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public IEnumerator AfterDefeatRoutine()
    {
        Time.timeScale = 1;
        defeatPanel.SetActive(false);
        semiTransperantImage.gameObject.SetActive(false);
        lostTxt.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        GameManager.Instance.LoadSceneWithNameFrom("LevelSelect", LoadingFrom.DEFEAT);

        yield return null;
    }
}