using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController instance;

    [SerializeField] private PowersArray[] powers;

    float acumWeight;
    float random;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        AccumulatedWieht();
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

    private void AccumulatedWieht()
    {
        foreach (var item in powers)
        {
            acumWeight += item.weight;
            item.accumWeight = acumWeight;
        }
    }
    public PowersArray GetValue(int index)
    {
        // Perform any validation checks here.
        return powers[index];
    }
}

[System.Serializable]
public class PowersArray
{
    public GameObject powerPrefab;
    public float weight;
    [HideInInspector] public float accumWeight;
}