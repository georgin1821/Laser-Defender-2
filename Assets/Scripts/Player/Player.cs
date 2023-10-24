using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : SimpleSingleton<Player>, IDamageable
{

    [Header("VFX")]
    [SerializeField] ParticleSystem engineVFX;
    [SerializeField] ParticleSystem shootingFlames;
    [SerializeField] GameObject rocketPrefab;

    [Space(10)]
    [Header("GameDev Settings")]
    public bool isAlwaysShooting = true;
    [SerializeField] bool collideWithEnemy = true;
    public bool isAbleToFire = true;

    [Tooltip("Shooting")]
    public Transform firePos;
    private int rocketsShooting = 7;
    private float arcAngle = 40;
    public GameObject[] targets; //to  be hide from inspector

    private AudioSource audioSource;
    private Animator anim;
    private bool playerHasShield;
    private Coroutine co;
    private IPlayerWeapon[] weapons;
    private int gunUpgrades = 1;
    private int shieldsDuration = 5;

    override protected void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        weapons = GetComponents<IPlayerWeapon>();
    }
    private void OnEnable()
    {
        if (GamePlayController.IsInitialized)
        {
            GamePlayController.OnGameStateChange += GameStateChangeHandle;
        }
        else
        {
            Debug.Log("GamePlayController instance is null");
        }
    }
    override protected void OnDestroy()
    {
        base.OnDestroy();
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }
    private void Start()
    {
        StopShootingClip();
        audioSource.loop = true;
        targets = new GameObject[rocketsShooting];
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FireRockets();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (!collideWithEnemy || playerHasShield) return;
            if (GamePlayController.Instance.state == GameState.PLAY && other.tag == "Enemy")
            {
                PlayerDeath();
            }
        }
    }

    public void StopShootingClip()
    {
        audioSource.Stop();
    }

    private void PlayerDeath()
    {
        GamePlayController.Instance.UpdateState(GameState.PLAYERDEATH);
        AudioController.Instance.PlayAudio(AudioType.PlayerDeath);
        Destroy(gameObject);
    }
    void GameStateChangeHandle(GameState state)
    {
        anim.enabled = (state != GameState.PLAY);

        if (state == GameState.PLAY)
        {
            audioSource.Play();
            engineVFX.Play();
            shootingFlames.Play();
        }
        if (state == GameState.INIT)
        {
            foreach (var weapon in weapons)
            {
                weapon.WeaponUpgrades = 1;
            }
        }
    }
    public void FireRockets()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < rocketsShooting; i++)
            {
                GameObject rocket = Instantiate(rocketPrefab, firePos.position, Quaternion.identity);
                rocket.transform.Rotate(0, 0, arcAngle - i * 15);
                AudioController.Instance.PlayAudio(AudioType.PalyerShootRockets);
            }
        }
    }

    public void ShieldsUp()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            if (co != null)
            {
                StopCoroutine(co);
            }
            co = StartCoroutine(ShieldsCountDown());
        }
    }

    public void UpgradeGun()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            foreach (var weapon in weapons)
            {
                weapon.WeaponUpgrades++;
                gunUpgrades++; // the same ranks for all weapons and same maximum
            }

            GameUIController.Instance.UpdateWeaponRankStatus(gunUpgrades);
        }
    }


    public void DamagePlayer()
    {
        if (!collideWithEnemy || playerHasShield) return;

        PlayerDeath();
    }
    public void PlayerAnimation()
    {
        //Speed Level debug
        if (GameManager.Instance.isSpeedLevel)
        {
            return;
        }
        else
        {
            anim.Play("Intro");
        }
    }
    IEnumerator ShieldsCountDown()
    {
        playerHasShield = true;
        GameObject shield = transform.Find("Shields").gameObject;
        shield.SetActive(true);
        AudioController.Instance.PlayAudio(AudioType.PlayerShields);
        yield return new WaitForSeconds(shieldsDuration);
        playerHasShield = false;
        shield.SetActive(false);
    }
}

