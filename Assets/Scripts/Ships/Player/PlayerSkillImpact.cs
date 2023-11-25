using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillImpact : PlayerProjectileImpact
{
    public AudioType impactClip;

    override public void ImapctProcess(Transform pos)
    {
        GameObject explotion = Instantiate(impactVFX, transform.position, Quaternion.identity);
            if (impactClip != AudioType.None)
            {
                AudioController.Instance.PlayAudio(impactClip);
            }

        Destroy(explotion, 1f);

    }
}
