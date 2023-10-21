using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DivisionConfiguration
{
    public GeneralSettings general;
    public SmoothDeltaSettings smooth;
    public RotationSettings rotation;
    public SpawnsSettings spawns;
    public FormMoveSettings formMove;
    public EnemyAISettings aISettings;


    [System.Serializable]
    public class GeneralSettings
    {
        public GameObject path;
        public GameObject formation;
        public GameObject[] enemyPrefabs;
        public bool isFormMoving;
        public bool isRotating;
        public bool endlessMove;
        public bool isChasingPlayer;
        public bool isSwampForm;
    }
    [System.Serializable]
    public class RotationSettings
    {
        public float rotationSpeed;
    }

    [System.Serializable]
    public class SmoothDeltaSettings
    {
        public float smoothDelta;
        public bool smoothMovement;
    }
    [System.Serializable]
    public class SpawnsSettings
    {
        public float moveSpeed;
        public int numberOfEnemies;
        public float timeBetweenSpawns;
    }

    [System.Serializable]
    public class FormMoveSettings
    {
        public AnimationCurve curve;
        [Range(0, 1)] public float magitude;
        [Range(0, 1)] public float frequency;
        public bool isMovingHorizontal;
        public bool isMovingVertical;
    }

    [System.Serializable]
    public class EnemyAISettings
    {
        public int AiChanceToReact;
        public float aiSpeed;
    }
    [System.Serializable]
    public class Swamp
    {

    }
}


