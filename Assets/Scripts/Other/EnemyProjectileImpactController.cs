using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileImpactController : MonoBehaviour
{

    /// ? needs that?
    /// </summary>
    [SerializeField] AudioType impactAudio;

    private void Start()
    {
        SetLevelOfDifficulty();
    }


    public void ImapctProcess()
    {
        VFXController.instance.EnemyLaserImpact(transform);

        if (impactAudio != AudioType.None)
        {
            AudioController.Instance.PlayAudio(impactAudio);
        }
        Destroy(gameObject);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.Instance.Difficulty;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.Instance.DamagePlayer();
            VFXController.instance.EnemyLaserImpact(transform);

            if (impactAudio != AudioType.None)
            {
                AudioController.Instance.PlayAudio(impactAudio);
            }
            Destroy(gameObject);
        }
    }
}
