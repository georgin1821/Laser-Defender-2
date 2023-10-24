using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileImpactController : MonoBehaviour
{

    [SerializeField] int damage = 100; /// <summary>
    /// ? needs that?
    /// </summary>
    [SerializeField] AudioType audioType;

    private void Start()
    {
        SetLevelOfDifficulty();
    }


    public void ImapctProcess()
    {
        VFXController.instance.EnemyLaserImpact(transform);

        if (audioType != AudioType.None)
        {
            AudioController.Instance.PlayAudio(audioType);
        }
        Destroy(gameObject);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.Instance.Difficulty;
        damage += (int)(diff * .5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.Instance.DamagePlayer();
            VFXController.instance.EnemyLaserImpact(transform);

            if (audioType != AudioType.None)
            {
                AudioController.Instance.PlayAudio(audioType);
            }
            Destroy(gameObject);
        }
    }
}
