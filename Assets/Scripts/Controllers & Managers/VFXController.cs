using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : SimpleSingleton<VFXController>
{

    [SerializeField] GameObject enemyLaserImpactVFX;
    [SerializeField] GameObject enemyDeath;



    public void EnemyLaserImpact(Transform trans)
    {
        GameObject explosion = Instantiate(enemyLaserImpactVFX, trans.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }

    public void EnemyDeath(Transform trans)
    {
        GameObject explosion = Instantiate(enemyDeath, trans.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }

}
