using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerWeapon : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int gunUpgrades;

    public int WeaponUpgrades
    {
        //starting from 1 to 10
        get { return gunUpgrades; }
        set
        {
            gunUpgrades = value;
            if (gunUpgrades > 10) gunUpgrades = 10;
        }

          }
    protected void SetGunUpgrade(int gunUpgrades)
    {

    }
}
