using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : SimpleSingleton<PowerUpController>
{

    [SerializeField] private PowersArray[] powers;

    private float acumWeight;
    private float random;

    private void Start()
    {
        AccumulatedWeight();
    }

    public void InstatiateRandomPower(Transform posTrnsform)
    {
        var power = Instantiate(GetRandomPower(), posTrnsform.position, Quaternion.identity);
    }

    public  GameObject GetRandomPower()
    {
         random = Random.Range(0f, 1f) * acumWeight;
        // Propability();
        foreach (var item in powers)
        {
            if(item.accumWeight >= random)
            {
                return item.powerPrefab;
            }
        }
            return null;
    }

    private void AccumulatedWeight()
    {
        foreach (var item in powers)
        {
            acumWeight += item.weight;
            item.accumWeight = acumWeight;
        }
    }
}

[System.Serializable]
public class PowersArray
{
    public GameObject powerPrefab;
    public float weight;
    [HideInInspector] public float accumWeight;
}