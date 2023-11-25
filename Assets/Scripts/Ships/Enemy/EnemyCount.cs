using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{

    public static EnemyCount instance;
   // public int Count { get; set; }

    public int Count;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public  void CountEnemiesAtScene(int number)
    {
        Count += number;
    }



}
