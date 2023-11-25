using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
  [HideInInspector]  public int LevelScore { get; set; }
    [HideInInspector] public int LevelCoins { get; set; }
    [HideInInspector] public int LevelIndex { get; set; }
    [HideInInspector] public DateTime currentTime { get; set; }
    [HideInInspector] public DateTime nextSessionTime;
    //Data
    [HideInInspector] public int selectedShip;
    [HideInInspector] public int gems;
    [HideInInspector] public int batteryLife;
    [HideInInspector] public bool isGameStartedFirstTime;
    [HideInInspector] public List<bool> levels;
    [HideInInspector] public LevelCompletedDifficulty[] levelCompletedDifficulty;
    [HideInInspector] public int coins;
     private int shipsCount = 5;
    [HideInInspector] private int levelsCount = 15;
    [HideInInspector] private int levelsComplete = 0;
    [HideInInspector] public int squadsUnlocked = 1;
    [HideInInspector] public int[] squad;
    [HideInInspector] public List<bool> unlockedShips;  //bool to true to unlock a ship in the array
    [HideInInspector] public List<int> shipsPower;
    [HideInInspector] public List<int> shipsRank;
    [HideInInspector] public List<string> shipsName;
    [HideInInspector] public DateTime sessionTime;
    [HideInInspector] public int enemiesKilled;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float soundVolume;
    [HideInInspector] public GameDifficulty currentDifficulty;
    [HideInInspector] public bool[] dailyRewards;
    [HideInInspector] public bool[][] shipsSkills;
    [HideInInspector] public int CurrentLevel { get; set; }
    public List<GameObject> shipsPrefab; 

    private GameData data;
    private LevelCompletedDifficulty gameDifficulty;

    protected override void Awake()
    {
        base.Awake();
        InitializeGameDate();
    }
    void Start()
    {
        CurrentLevel = 0;
    }
    private void OnApplicationQuit()
    {
        Save();
    }

    void InitializeGameDate()
    {
        currentTime = DateTime.UtcNow;

        Load();
        if (data != null)
        {
            isGameStartedFirstTime = data.IsGameStartedFirstTime;
            nextSessionTime = DateTime.UtcNow;
            SessionController.instance.RewardCheckOnStart(sessionTime, nextSessionTime);
            sessionTime = nextSessionTime;
        }
        else
        {
            isGameStartedFirstTime = true;
        }

        if (isGameStartedFirstTime)
        {
            sessionTime = DateTime.UtcNow;
            coins = 200;
            gems = 10;
            batteryLife = 90;
            enemiesKilled = 0;
            levels = new List<bool>();
            levelCompletedDifficulty = new LevelCompletedDifficulty[5];
            shipsCount = Enum.GetNames(typeof(ShipsNameEnum)).Length;
            unlockedShips = new List<bool>();
            shipsPower = new List<int>();
            shipsRank = new List<int>();
            dailyRewards = new bool[4];
            currentDifficulty = GameDifficulty.EASY;
            musicVolume = 0.7f;
            soundVolume = 0.5f;
            isGameStartedFirstTime = false;

            UnlockSquads();
            SetShipNames();
            squad = new int[3] { 0, 1, 2 }; // squad
            foreach (var item in shipsName)
            {
                // Debug.Log(item);
            }
            shipsPower.Insert(shipsName.IndexOf("startingShip"), 100);
            shipsPower.Insert(shipsName.IndexOf("ship2"), 100);
            shipsPower.Insert(shipsName.IndexOf("ship3"), 100);
            shipsPower.Insert(shipsName.IndexOf("ship4"), 100);

            for (int i = 0; i < shipsCount - 1; i++)
            {
                shipsRank.Add(0);
            }

            unlockedShips.Add(true);
            for (int i = 1; i < shipsCount - 1; i++)
            {
                unlockedShips.Add(false);
            }

            levels.Add(true);
            for (int i = 1; i < levelsCount - 1; i++)
            {
                levels.Add(false);
            }
            levels[1] = true; //test

            //unlocking first ship locks the others
            for (int i = 0; i < dailyRewards.Length; i++)
            {
                dailyRewards[i] = false;
            }
            for (int i = 0; i < levelCompletedDifficulty.Length; i++)
            {
                levelCompletedDifficulty[i] = LevelCompletedDifficulty.NONE;
            }

            //test
            // levelCompletedDifficulty[0] = LevelCompletedDifficulty.MEDIUM;
            //  levelCompletedDifficulty[1] = LevelCompletedDifficulty.EASY;


            unlockedShips[0] = true;
            unlockedShips[1] = true;

            data = new GameData();

            data.Coins = coins;
            data.IsGameStartedFirstTime = isGameStartedFirstTime;
            data.Levels = levels;
            data.Squad = squad;
            data.UnlockedShips = unlockedShips;
            data.ShipsPower = shipsPower;
            data.SelectedShip = selectedShip;
            data.ShipsName = shipsName;
            data.ShipsRank = shipsRank;
            data.SquadsUnlocked = squadsUnlocked;
            data.CurrentDifficulty = currentDifficulty;
            data.SessionTime = sessionTime;
            data.EnemiesKilled = enemiesKilled;
            data.MusicVolume = musicVolume;
            data.SoundVolume = soundVolume;
            data.DailyRewards = dailyRewards;
            data.Squad = squad;
            data.ShipsPrefab = shipsPrefab;


            Save();
            Load();
        }
    }
    public void Save()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/GamaData.dat");
            if (data != null)
            {
                data.Coins = coins;
                data.Gems = gems;
                data.IsGameStartedFirstTime = isGameStartedFirstTime;
                data.Levels = levels;
                data.Squad = squad;
                data.UnlockedShips = unlockedShips;
                data.ShipsRank = shipsRank;
                data.ShipsName = shipsName;
                data.SquadsUnlocked = squadsUnlocked;

                data.SelectedShip = selectedShip;
                data.CurrentDifficulty = currentDifficulty;
                data.BatteryLife = batteryLife;
                data.SessionTime = sessionTime;
                data.EnemiesKilled = enemiesKilled;
                data.MusicVolume = musicVolume;
                data.SoundVolume = soundVolume;
                data.Skills = shipsSkills;
                data.DailyRewards = dailyRewards;
                data.Squad = squad;
                data.ShipsPrefab = shipsPrefab;

                bf.Serialize(file, data);
            }
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
    private void Load()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            data = (GameData)bf.Deserialize(file);
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    private void SetShipNames()
    {
        shipsName = ((ShipsNameEnum[])Enum.GetValues(typeof(ShipsNameEnum))).Select(c => c.ToString()).ToList();
    }
    private void UnlockSquads()
    {
        if (levelsComplete < 5)
        {
            squadsUnlocked = 1;
        }
    }
}

public enum GameDifficulty
{
    EASY = 1,
    MEDIUM = 2,
    HARD = 3
}

public enum LevelCompletedDifficulty
{
    NONE = 0,
    EASY = 1,
    MEDIUM = 2,
    HARD = 3
}
public enum ShipsNameEnum
{
    startingShip,
    ship2,
    ship3,
    ship4,
    ship5
}
[Serializable]
class GameData
{
    public List<GameObject> ShipsPrefab { get; set; }

    public int SelectedShip { get; set; }
    public int[] Squad { get; set; }
    public int Coins { get; set; }
    public List<bool> UnlockedShips { get; set; }
    public List<int> ShipsPower { get; set; }
    public List<string> ShipsName { get; set; }
    public List<int> ShipsRank { get; set; }
    public int Gems { get; set; }
    public int SquadsUnlocked { get; set; }
    public bool IsGameStartedFirstTime { get; set; }
    public List<bool> Levels { get; set; }
    public int BatteryLife { get; set; }
    public DateTime SessionTime { get; set; }
    public GameDifficulty CurrentDifficulty { get; set; }
    public int EnemiesKilled { get; set; }
    public float MusicVolume { get; set; }
    public float SoundVolume { get; set; }
    public bool[] DailyRewards { get; set; }
    public bool[][] Skills { get; set; }
    public LevelCompletedDifficulty[] levelCompletedDifficulty { get; set; }


}
