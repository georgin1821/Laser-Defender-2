using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionSpawn : MonoBehaviour
{
    public static event Action OnLastEnemyAtFormation;
    [SerializeField] DivisionStartingState divSet;

    [HideInInspector] public List<Transform> waypoints;
    [HideInInspector] public List<Transform> formationWaypoints;

    [SerializeField] float preDelay;
    [SerializeField] DivisionConfiguration divisionConfig;

    GameObject[] enemyPrefabs;
    int index;
    private void OnValidate()
    {
        switch (divSet)
        {
            case DivisionStartingState.Endless:
                divisionConfig.general.endlessMove = true;
                divisionConfig.general.isFormMoving = false;
                divisionConfig.general.formation = null;
                divisionConfig.formMove = null;
                divisionConfig.aISettings = null;
                divisionConfig.general.isChasingPlayer = false;
                divisionConfig.general.isSwampForm = false;
                break;
            case DivisionStartingState.Formation:
                divisionConfig.general.endlessMove = false;
                break;
            case DivisionStartingState.TwoPoints:
                divisionConfig.general.endlessMove = false;
                divisionConfig.smooth.smoothMovement = true;
                break;
            case DivisionStartingState.ChasingPlayer:
                divisionConfig.general.isChasingPlayer = true;
                divisionConfig.general.isFormMoving = false;
                divisionConfig.general.endlessMove = false;
                divisionConfig.formMove = null;

                break;
        }
    }
    void Start()
    {
        waypoints = new List<Transform>();
        formationWaypoints = new List<Transform>();

        if (divisionConfig.general.path != null)
        {
            foreach (Transform child in divisionConfig.general.path.transform)
            {
                waypoints.Add(child);
            }
        }

        if (divisionConfig.general.formation != null)
        {
            foreach (Transform child in divisionConfig.general.formation.transform)
            {
                formationWaypoints.Add(child);
            }
        }

        EnemyCount.instance.CountEnemiesAtScene(divisionConfig.spawns.numberOfEnemies);
        StartCoroutine(InstantiateDivision());
    }
    IEnumerator InstantiateDivision()
    {
        EnemyPathfinding ep = null;
        yield return new WaitForSeconds(preDelay);
        enemyPrefabs = divisionConfig.general.enemyPrefabs;
        for (index = 0; index < divisionConfig.spawns.numberOfEnemies; index++)
        {
            GameObject newEnemy = InstatiatePrefab(divSet);
            ep = newEnemy.GetComponent<EnemyPathfinding>();
            ep.SetDivisionConfiguration(divisionConfig, this, id: index);

            switch (divSet)
            {
                case DivisionStartingState.Endless:
                    ep.StartingState(EnemyStartingState.DivisionToPath);
                    break;

                case DivisionStartingState.Formation:
                    ep.StartingState(EnemyStartingState.DivisionToPath);
                    break;

                case DivisionStartingState.TwoPoints:
                    ep.StartingState(EnemyStartingState.PointToPoint);
                    break;
                case DivisionStartingState.ChasingPlayer:
                    ep.StartingState(EnemyStartingState.ChasingPlayer);
                    break;
            }
            yield return new WaitForSeconds(divisionConfig.spawns.timeBetweenSpawns);
        }
        ep.OnPositionForamtion += Handler;
    }

    private void Handler()
    {
        OnLastEnemyAtFormation?.Invoke();
    }

    private GameObject InstatiatePrefab(DivisionStartingState set)
    {
        int i = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        int idPos;
        if (set == DivisionStartingState.TwoPoints) idPos = index;
        else idPos = 0;

        return Instantiate(enemyPrefabs[i],
                           waypoints[idPos].position,
                           enemyPrefabs[i].transform.rotation);
    }
}
public enum DivisionStartingState
{
    Endless,
    Formation,
    TwoPoints,
    ChasingPlayer
}