using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public  event Action<float> OnGettingDamage;

    [Header("Enemy Stats")]
    public float health = 100;
    [SerializeField] int scoreValue = 150;

    [SerializeField] GameObject projectilePrefab;
    [Header("VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioType shipSound;

    [Header("PowerUps")]
    [SerializeField] int chanchToDropPower;
    [SerializeField] int chanceOfDropingGold;
    [SerializeField] int chanceOfDropMultipleCoins;
    [SerializeField] int chanseOfDropGems;
    [SerializeField] int cointToSpwan;

    bool isNotAlive;

    private void Start()
    {
        SetLevelOfDifficulty();
        if(shipSound != AudioType.None)
        {
            AudioController.Instance.LoopAudio(shipSound, true, 1);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerProjectileImpact impactProcess = other.gameObject.GetComponent<PlayerProjectileImpact>();
        if (impactProcess == null) { return; }
        ProcessHit(impactProcess);
    }

    public void ProcessHit(PlayerProjectileImpact impactProcess)
    {
        health -= impactProcess.GetDamage(); ;
        OnGettingDamage?.Invoke(health);
        impactProcess.ImapctProcess(gameObject.transform);

        if (health <= 0 && !isNotAlive)
        {
            Destroy(impactProcess.gameObject);
            isNotAlive = true;
            Die();
            OnDieDropPower();
            OnDieDropGold();
            OnDiewDropGems();
        }
    }
    public void ProccessHitLaser(float damage)
    {
        OnGettingDamage?.Invoke(health);
        health -= damage;

        if (health <= 0 && !isNotAlive)
        {
            isNotAlive = true;
            Die();
            OnDieDropPower();
            OnDieDropGold();
            OnDiewDropGems();
        }

    }

    private void OnDieDropGold()

    {
        bool multipleCoins;
        if (UnityEngine.Random.Range(1, 100) <= chanceOfDropingGold)
        {
            multipleCoins = UnityEngine.Random.Range(1, 100) <= chanceOfDropMultipleCoins;
            Coins.instance.DropGold(this.transform, multipleCoins, cointToSpwan);
        }
    }
    private void OnDieDropPower()
    {
        if (UnityEngine.Random.Range(1, 100) <= chanchToDropPower)
        {
            PowerUpController.instance.InstatiateRandomPower(this.transform);
        }
    }
    private void OnDiewDropGems()
    {
        if (UnityEngine.Random.Range(1, 100) <= chanchToDropPower)
        {
            PowerUpController.instance.InstatiateRandomPower(this.transform);
        }
    }
    private void Die()
    {
        EnemyCount.instance.Count--;
        GamePlayController.Instance.AddToScore(scoreValue);
        AudioController.Instance.PlayAudio(AudioType.EnemyDeathSound);
        VFXController.instance.EnemyDeath(transform);
        Destroy(gameObject);
    }
    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.Instance.Difficulty;
        health += health * diff;
        scoreValue += (int)(diff * .5f);
    }
}
