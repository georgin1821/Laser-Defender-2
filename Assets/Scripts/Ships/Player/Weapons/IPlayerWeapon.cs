using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerWeapon : MonoBehaviour
{
   [SerializeField][Range(0, 1)] private int weaponUpgrades;

    public int WeaponUpgrades
    {
        //starting from 1 to 10
        get { return weaponUpgrades; }
        set
        {
            weaponUpgrades = value;
            if (weaponUpgrades > 10) weaponUpgrades = 10;
        }

          }
    protected void SetGunUpgrade(int gunUpgrades)
    {

    }
}
