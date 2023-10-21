using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionChasingPlayerSpawn : MonoBehaviour
{
    public float speed;
    [SerializeField] int shipsCount;
    [SerializeField] GameObject path;
    public GameObject[] enemyPrefabs;
    public float timeBetweenSpawn;

    [HideInInspector] public List<Transform> waypoints;
    void Start()
    {
        waypoints = new List<Transform>();

        if (path != null)
        {
            foreach (Transform child in path.transform)
            {
                waypoints.Add(child);
            }
        }
    }

    IEnumerator InstatiateWave()
    {
        for (int index = 0; index < shipsCount; index++)
        {
            int i = Random.Range(0, enemyPrefabs.Length);

            GameObject newEnemy = Instantiate(enemyPrefabs[i], waypoints[index].position, enemyPrefabs[i].transform.rotation);

            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();

          //  ep.SetDivisionConfiguration(this);

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

}
