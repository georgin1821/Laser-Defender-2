using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    public static LevelSpawner instance;

    [SerializeField] List<LevelConfig> levels;

    public List<LevelConfig> Levels { get { return levels; } }

    public int LevelIndex { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnLevelWithIndex(int index)
    {
        if (index < levels.Count)
        {
            WaveSpawner.Instance.SetWaves(levels[index].Waves);
            WaveSpawner.Instance.SpawnTheLevel();
        }
    }
}
