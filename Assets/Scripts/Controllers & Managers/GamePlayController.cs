using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public sealed class GamePlayController : SimpleSingleton<GamePlayController>
{
    //public static GamePlayController instance;

    [Header("Prefabs")]
    private GameObject[] shipsPrefabs;

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
    GameObject playerPrefab;
    private Vector3 shipStartingPos = new Vector3(0, -10, 0);
    private int levelScore;
    private int currentSquadShip = 0;

    //int batterySpend;
    public int ShipPower { get; private set; }

    [HideInInspector] public bool andOfAnimation;

    protected override void Awake()
    {
        base.Awake();
        // GameDataManager.Instance.Save();
    }


    private void Start()
    {
        shipsPrefabs = new GameObject[GameDataManager.Instance.squad.Length];

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
        SetSquadPrefabs();
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
                WaveSpawner.Instance.DestroyWaves();

                //Instantiation
                playerPrefab = shipsPrefabs[currentSquadShip];
                ShipPower = GameDataManager.Instance.shipsPower[currentSquadShip];

                InstantiateScrollingBackgrounds();
                InstantiatePlayer();

                BeginIntroSequence(false);

                // batterySpend = 0;
                Score = 0;
                GameUIController.Instance.UpdateScore(Score);

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
                Player.Instance.StopShootingClip();

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

                StartCoroutine(LoadNextLvlWithDelay());
                break;

            case GameState.PLAYERDEATH:
                currentSquadShip++;
                if (currentSquadShip >= shipsPrefabs.Length)
                {
                    UpdateState(GameState.DEFEAT);
                    return;
                }
                Destroy(Player.Instance.gameObject);
                Player.Instance.DestroySingleton();
                PlayerController.Instance.DestroySingleton();
                playerPrefab = shipsPrefabs[currentSquadShip];
                Instantiate(playerPrefab, shipStartingPos, Quaternion.identity);
                StartCoroutine(PlayerStartingAnim(false));
                Player.Instance.ShieldsUp();
                UpdateState(GameState.PLAY);
                break;

            case GameState.DEFEAT:
                Time.timeScale = 0;
                Player.Instance.StopShootingClip();
                // WaveSpawner.Instance.StopAllCoroutines();
                break;

            case GameState.LEVELCOMPLETE_UI:
                break;

            case GameState.RETRY:
                Time.timeScale = 1;
                UpdateState(GameState.INIT);
                break;

            case GameState.PAUSE:
                Time.timeScale = 0;
                Player.Instance.StopShootingClip();
                break;

            case GameState.EXIT:
                AudioController.Instance.StopAudio(soundtrack, true);
                break;
        }

        OnGameStateChange?.Invoke(state);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.INIT:
                if (andOfAnimation) UpdateState(GameState.LOADLEVEL);
                break;
        }
    }

    public void AddToScore(int scoreValue)
    {
        int startScore = Score;
        Score += scoreValue;
        levelScore += scoreValue;
        StartCoroutine(GameUIController.Instance.UpdateScoreRoutine(startScore, Score));
    }
    //LevelUpSystem
    private void SetSquadPrefabs()
    {

        for (int i = 0; i < GameDataManager.Instance.squad.Length; i++)
        {
            shipsPrefabs[i] = GameDataManager.Instance.shipsPrefab[GameDataManager.Instance.squad[i]];
        }

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
        Player player = FindAnyObjectByType<Player>();

        if (player == null)
        {
            Instantiate(playerPrefab, shipStartingPos, Quaternion.identity);
        }
    }
    public void BeginIntroSequence(bool isRiviving)
    {
        StartCoroutine(PlayerStartingAnim(isRiviving));
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
    private IEnumerator PlayerStartingAnim(bool isRiviving)
    {
        andOfAnimation = false;


        if (!GameManager.Instance.isSpeedLevel)
        {
            AnimationClip clip = Player.Instance.GetComponent<Animator>().runtimeAnimatorController.animationClips[0];
            float animtime = clip.length;
            float t;
            if (isRiviving)
            {
                t = 0;
            }
            else
            {
                t = 1;
            }
            yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(t));
            Player.Instance.PlayerAnimation();


            yield return new WaitForSecondsRealtime(animtime);
            andOfAnimation = true;
        }
        else
        {
            yield return null;
            andOfAnimation = true;

        }

    }
    private IEnumerator LoadNextLvlWithDelay()
    {
        yield return new WaitForSecondsRealtime(3);
        UpdateState(GameState.LEVELCOMPLETE_UI);
        GameManager.Instance.loadingFrom = LoadingFrom.LVLCOMP;
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");

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
    PLAYERDEATH,
    DEFEAT,
    LEVELCOMPLETE_UI,
    EXIT
}