using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileImpactLong : PlayerProjectileImpact
{
    public override void ImapctProcess(Transform pos)
    {
        GameObject explotion = Instantiate(impactVFX, pos.position, Quaternion.identity);

        Destroy(explotion, 1f);
    }
}
