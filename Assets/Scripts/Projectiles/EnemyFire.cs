using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : EnemyWeaponAbstract
{

   [SerializeField] ParticleSystem fireParticle;
    [SerializeField] Transform fireTrans;

    GameObject projectile;
    private void Start()
    {
        InvokeRepeating("FireChance", minTimeToFire, maxTimeToFire);
    }

    protected override void FireChance()
    {
        if (UnityEngine.Random.Range(1, 100) <= chanceToFire)
        {
            Firing();
        }
    }

    public override void Firing()
    {
        fireParticle.Play();
        projectile = Instantiate(prjectilePrefab, fireTrans.position, Quaternion.identity);
    }
}
