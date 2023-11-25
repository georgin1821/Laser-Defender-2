using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsWeaponAngle : PlayerBulletsAbstract
{
    [SerializeField] private int angleBetweenLasers = 5;

    protected override void InstatiateProjectiles(int upgrades)
    {
        int angle = 0;
        angle = angleBetweenLasers * (upgrades - 1) / 2;

        for (int i = 0; i < upgrades; i++)
        {
            PlayerProjectile progectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, angle));
            angle = angle - angleBetweenLasers;
        }
    }
}
