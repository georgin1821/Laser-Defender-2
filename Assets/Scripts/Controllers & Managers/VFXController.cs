using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController instance;

    [SerializeField] GameObject enemyLaserImpactVFX;
    [SerializeField] GameObject enemyDeath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void EnemyLaserImpact(Transform trans)
    {
        GameObject explosion = Instantiate(enemyLaserImpactVFX, trans.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }

    public void EnemyDeath(Transform trans)
    {
        StartCoroutine(PlayVFX(trans));
    }

    IEnumerator PlayVFX(Transform trans)
    {
        GameObject explosion = Instantiate(enemyDeath, trans.position, Quaternion.identity);
        Destroy(explosion, 1f);
        yield return null;
    }
}
