using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgradeController : PlayerPowersControllerAbstract
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Player.instance.UpgradeGun();
    }
}
