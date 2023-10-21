using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocketsController : PlayerPowersControllerAbstract
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Player.instance.FireRockets();

    }
}
