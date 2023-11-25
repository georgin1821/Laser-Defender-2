using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base Class for enemy weapons
public class PlayerProjectileImpact : MonoBehaviour
{
    [SerializeField] private int damage = 100;
    [SerializeField] protected GameObject impactVFX;
    public int GetDamage()
    {
        //LevelUp System
        damage += GamePlayController.Instance.ShipPower / 10;
        return damage;
    }

    public virtual void ImapctProcess(Transform pos)
    {
        GameObject explotion = Instantiate(impactVFX, transform.position, Quaternion.identity);

        Destroy(explotion, 1f);

        Destroy(gameObject);
    }
}
