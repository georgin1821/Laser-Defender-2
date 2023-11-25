using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public  event Action<float> OnGettingDamage;

    [SerializeField] GameObject projectilePrefab;

    [Header("Enemy Stats")]
    public float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("VAFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioType shipSound;

    [Header("PowerUps")]
    [SerializeField] int chanchToDropPower;
    [SerializeField] int chanceOfDropingGold;
    [SerializeField] int chanceOfDropMultipleCoins;
    [SerializeField] int chanseOfDropGems;
    [SerializeField] int cointToSpawn;

    private bool isNotAlive;

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
        PlayerProjectileImpact impact = other.gameObject.GetComponent<PlayerProjectileImpact>();
        if (impact == null) { return; }
        ProcessHit(impact);
    }

    public void ProcessHit(PlayerProjectileImpact impact)
    {
        health -= impact.GetDamage(); ;
        OnGettingDamage?.Invoke(health);
        impact.ImapctProcess(gameObject.transform);

        if (health <= 0 && !isNotAlive)
        {
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
            Coins.instance.DropGold(this.transform, multipleCoins, cointToSpawn);
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
    private void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.Instance.Difficulty;
        health += health * diff;
        scoreValue += (int)(diff * .5f);
    }
}
