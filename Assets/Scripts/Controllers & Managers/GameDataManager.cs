using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public int LevelScore { get; set; }
    public int LevelCoins { get; set; }
    public int LevelIndex { get; set; }
    public DateTime currentTime { get; set; }
    public DateTime nextSessionTime;

    //Data
    public int selectedShip;
    public bool[] ships;  //bool to true to unlock a ship in the array
    public int coins;
    public int gems;
    public int batteryLife;
    public bool isGameStartedFirstTime;
    public bool[] levels;
    public LevelCompletedDifficulty[] levelCompletedDifficulty;
    public int[] shipsPower;
    public int[] shipsRank;
    public DateTime sessionTime;
    public int enemiesKilled;
    public float musicVolume;
    public float soundVolume;
    public GameDifficulty currentDifficulty;
    public bool[] dailyRewards;
    public bool[][] shipsSkills;
    public int CurrentLevel { get; set; }

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
            levels = new bool[5];
            levelCompletedDifficulty = new LevelCompletedDifficulty[5];
            ships = new bool[5];
            shipsPower = new int[6];
            dailyRewards = new bool[4];
            currentDifficulty = GameDifficulty.EASY;
            musicVolume = 0.7f;
            soundVolume = 0.5f;

            for (int i = 0; i < shipsPower.Length; i++)
            {
                shipsPower[0] = 100;
                shipsPower[1] = 200;
            }

            isGameStartedFirstTime = false;

            levels[0] = true;
            for (int i = 1; i < levels.Length; i++)
            {
                levels[i] = false;
            }
            levels[1] = true; //test


            //unlocking first ship locks the others
            ships[0] = true;
            for (int i = 1; i < ships.Length; i++)
            {
                ships[i] = false;
            }

            shipsRank = new int[5];
            for (int i = 0; i < shipsRank.Length; i++)
            {
                shipsRank[i] = 0;
            }
            for(int i = 0; i < dailyRewards.Length; i++)
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


            ships[0] = true;
            ships[1] = true;

            data = new GameData();

            data.Coins = coins;
            data.IsGameStartedFirstTime = isGameStartedFirstTime;
            data.Levels = levels;
            data.Ships = ships;
            data.ShipsPower = shipsPower;
            data.SelectedShip = selectedShip;
            data.ShipsRank = shipsRank;
            data.CurrentDifficulty = currentDifficulty;
            data.SessionTime = sessionTime;
            data.EnemiesKilled = enemiesKilled;
            data.MusicVolume = musicVolume;
            data.SoundVolume = soundVolume;
            data.DailyRewards = dailyRewards;

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
                data.Ships = ships;
                data.ShipsRank = shipsRank;
                data.SelectedShip = selectedShip;
                data.CurrentDifficulty = currentDifficulty;
                data.BatteryLife = batteryLife;
                data.SessionTime = sessionTime;
                data.EnemiesKilled = enemiesKilled;
                data.MusicVolume = musicVolume;
                data.SoundVolume = soundVolume;
                data.Skills = shipsSkills;
                data.DailyRewards = dailyRewards;

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
[Serializable]
class GameData
{
    public int SelectedShip { get; set; }
    public int Coins { get; set; }
    public bool[] Ships { get; set; }
    public int Gems { get; set; }
    public bool IsGameStartedFirstTime { get; set; }
    public bool[] Levels { get; set; }
    public int[] ShipsPower { get; set; }
    public int[] ShipsRank { get; set; }
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
