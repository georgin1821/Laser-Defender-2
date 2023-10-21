using System;
using System.Collections;
using UnityEngine;

public sealed class GamePlayController : Singleton<GamePlayController>
{
    //public static GamePlayController instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] shipsPrefabs = new GameObject[1];

    private int aliveShips = 2; // draft for testing

    [SerializeField] private GameObject[] scrollingBgsPrefabs;

    public static event Action<GameState> OnGameStateChange;

    public static event Action OnScrollingBGEnabled;

    [Header("Info")]
    public GameState state; //to be serialize only

    public GameDifficulty gameDifficulty;
    [HideInInspector] public int Score;
    [HideInInspector] public int levelCoins;
    [HideInInspector] public float Difficulty;

    private AudioType victoryClip;
    private GameObject[] sbgGameObjects;
    private AudioType soundtrack;
    private GameObject playerPrefab;
    private Vector3 shipStartingPos = new Vector3(0, -6, 0);
    private int levelScore;

    //int batterySpend;
    public int ShipPower { get; private set; }

     protected override void Awake()
    {
        base.Awake();
        GameDataManager.Instance.Save();
    }

    protected override void InitializeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("[Singleton] Trying to instantitate a second instance of singleton");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitializeGame();

        victoryClip = AudioType.victory;
        soundtrack = LevelSpawner.instance.Levels[GameDataManager.Instance.CurrentLevel].MusicClip;
        PlayClipWithFade(soundtrack);

        UpdateState(GameState.INIT);
    }

    private void InitializeGame()
    {
        /* ------REFACTORING depends from other app systems-------
         */
        SelectShipPrefabSetShipPower();
        SetLevelDifficulty();
    }

    public void UpdateState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.INIT:
                Time.timeScale = 1;

                //Clears remaining enemies and sbgs
                DestroyLevel();
                WaveSpawner.instance.DestroyWaves();

                //Instantiation
                InstantiateScrollingBackgrounds();
                InstantiatePlayer();

                BeginIntroSequence();

                // batterySpend = 0;
                Score = 0;
                GameUIController.instance.UpdateScore(Score);

                EnemyCount.instance.Count = 0;
                levelScore = 0;
                levelCoins = 0;
                //update state to next only when player anim finishes

                break;

            case GameState.LOADLEVEL:

                LevelSpawner.instance.SpawnLevelWithIndex(GameDataManager.Instance.CurrentLevel);

                UpdateState(GameState.PLAY);
                break;

            case GameState.PLAY:
                Time.timeScale = 1;
                break;

            case GameState.LEVELCOMPLETE:
                Player.instance.StopShootingClip();

                int unlockedLevel = GameDataManager.Instance.CurrentLevel;
                if (GameDataManager.Instance.levels[unlockedLevel + 1] == false)
                {
                    GameDataManager.Instance.levels[unlockedLevel + 1] = true;
                }

                if (GameDataManager.Instance.levelCompletedDifficulty[unlockedLevel] < (LevelCompletedDifficulty)gameDifficulty)

                {
                    GameDataManager.Instance.levelCompletedDifficulty[unlockedLevel] = (LevelCompletedDifficulty)gameDifficulty;
                }
                GameDataManager.Instance.LevelCoins = levelCoins;
                GameDataManager.Instance.LevelScore = levelScore;
                GameDataManager.Instance.batteryLife -= 10;
                GameDataManager.Instance.LevelIndex = LevelSpawner.instance.LevelIndex;
                GameDataManager.Instance.Save();

                StopAudioWithFade(soundtrack);
                PlayClipWithFade(victoryClip);

                StartCoroutine(DelayRoutine());
                break;

            case GameState.DEFEAT:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                WaveSpawner.instance.StopAllCoroutines();
                break;

            case GameState.LEVELCOMPLETE_UI:
                break;

            case GameState.RETRY:
                Time.timeScale = 1;
                UpdateState(GameState.INIT);
                break;

            case GameState.PAUSE:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                break;

            case GameState.EXIT:
                AudioController.Instance.StopAudio(soundtrack, true);
                break;
        }

        OnGameStateChange?.Invoke(state);
    }

    public void AddToScore(int scoreValue)
    {
        int startScore = Score;
        Score += scoreValue;
        levelScore += scoreValue;
        StartCoroutine(GameUIController.instance.UpdateScore(startScore, Score));
    }

    private void SelectShipPrefabSetShipPower()
    {
        int index = GameDataManager.Instance.selectedShip;
        playerPrefab = shipsPrefabs[index];
        ShipPower = GameDataManager.Instance.shipsPower[index];
    }

    public void SetLevelDifficulty()
    {
        gameDifficulty = GameDataManager.Instance.currentDifficulty;

        switch (gameDifficulty)
        {
            case GameDifficulty.EASY:
                Difficulty = 0;
                break;

            case GameDifficulty.MEDIUM:
                Difficulty = 1.5f;
                break;

            case GameDifficulty.HARD:
                Difficulty = 3f;
                break;
        }
    }

    private void InstantiateScrollingBackgrounds()
    {
        sbgGameObjects = new GameObject[scrollingBgsPrefabs.Length];

        for (int i = 0; i < scrollingBgsPrefabs.Length; i++)
        {
            sbgGameObjects[i] = Instantiate(scrollingBgsPrefabs[i]);
        }

        OnScrollingBGEnabled?.Invoke();
    }

    private void InstantiatePlayer()
    {
        Player player = FindObjectOfType<Player>();

        if (player == null)
        {
            Instantiate(playerPrefab, shipStartingPos, Quaternion.identity);
        }
    }

    private void BeginIntroSequence()
    {
        StartCoroutine(PlayerStartingAnim());
    }

    private void DestroyLevel()
    {
        // destroy enemies objects and scrolling bgs objs at scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length != 0)
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
        }
        if (sbgGameObjects != null)
        {
            foreach (var sbg in sbgGameObjects)
            {
                Destroy(sbg);
            }
        }
    }

    private IEnumerator PlayerStartingAnim()
    {
        float t1, t2;

        if (GameManager.Instance.isSpeedLevel)
        {
            t1 = 0;
            t2 = 1;
        }
        else
        {
            t1 = 0.5f; t2 = 4f;
        }

        yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(t1));
        Player.instance.gameObject.SetActive(true);
        Player.instance.PlayerAnimation();

        yield return new WaitForSecondsRealtime(t2);
        UpdateState(GameState.LOADLEVEL);
    }

    private IEnumerator DelayRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        UpdateState(GameState.LEVELCOMPLETE_UI);
    }

    private void PlayClipWithFade(AudioType clip)
    {
        if (clip != AudioType.None)
        {
            AudioController.Instance.PlayAudio(clip, true);
        }
    }

    private void StopAudioWithFade(AudioType clip)
    {
        if (clip != AudioType.None)
        {
            AudioController.Instance.StopAudio(clip, true);
        }
    }
}

public enum GameState
{
    INIT,
    LOADLEVEL,
    PLAY,
    LEVELCOMPLETE,
    RETRY,
    PAUSE,
    DEFEAT,
    LEVELCOMPLETE_UI,
    EXIT
}