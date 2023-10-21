using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config")]

public class WaveConfig : ScriptableObject
{
    [SerializeField] List<GameObject> divisions;
    [SerializeField] float spawnDelay;
    public bool isBoss;

    public List<GameObject> GetDivisions()
    {
        return divisions;
    }
    public float GetDelay()
    {
        return spawnDelay;
    }

}
