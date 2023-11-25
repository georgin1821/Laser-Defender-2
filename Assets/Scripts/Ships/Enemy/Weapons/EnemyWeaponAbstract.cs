using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeaponAbstract : MonoBehaviour
{
    [SerializeField] protected GameObject prjectilePrefab;

    [Space(5)]
    [SerializeField] protected float chanceToFire;
    [SerializeField] protected int delayToShoot, repeatTime;

    abstract protected void FireChance();
    abstract public void Firing();

}
