using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss : MonoBehaviour
{
    public int health;
    [SerializeField] int scoreValue;
    [SerializeField] GameObject deathVFX;

    bool isNotAlive;

    private void Awake()
    {
        SetLevelOfDifficulty();
        GameUIController.instance.EnemyHealthSliderConfigure(health);
    }

    private void OnTriggerEnter(Collider other)
    {
       // PlayerProjectileImpact impactProcess = other.gameObject.GetComponent<PlayerProjectileImpact>();
      //  if (impactProcess == null) { return; }
       // ProcessHit(impactProcess);
    }

    private void ProcessHit(PlayerProjectileImpact impactProcess)
    {
        health -= impactProcess.GetDamage();
        if (health < 0) health = 0;
        GameUIController.instance.EnemyHealthSliderUpdate(health);
        impactProcess.ImapctProcess(gameObject.transform);

        if (health <= 0 && !isNotAlive)
        {
            isNotAlive = true;
            Die();
            OnDieDropGold();
        }
    }

    private void OnDieDropGold()

    {
        Coins.instance.DropGold(this.transform, true, 1);
    }

    private void Die()
    {
        EnemyCount.instance.Count--;
        GamePlayController.Instance.AddToScore(scoreValue);
        AudioController.Instance.PlayAudio(AudioType.EnemyDeathSound);
        // VFXController.instance.EnemyDeath(transform);
        Destroy(gameObject);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.Instance.Difficulty;
        health += (int)(health * diff);
        scoreValue += (int)(diff * .5f);
    }
}
