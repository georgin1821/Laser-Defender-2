using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable
{
    public static Player instance;

    [Header("VFX")]
    [SerializeField] ParticleSystem engineFlames;
    [SerializeField] ParticleSystem shootingFlames;
    [SerializeField] GameObject rocketPrefab;

    [Space(10)]
    [Header("GameDev Settings")]
    public bool isAlwaysShooting = true;
    [SerializeField] bool collideWithEnemy = true;

    [Tooltip("Fire posiyion")]
    public Transform firePos;
    [SerializeField] GameObject shieldsVFX;
    GameObject redFlashImage;

    AudioSource audioSource;
    Animator anim;
    float arcAngle = 40;
    private bool playerHasShield;
    bool isGameStatePLAY;
    Coroutine co;
    GameObject shields;
    public int Health = 100;
    public GameObject[] Targets { get; set; }
    //
    private int gunUpgrades = 1;
    public int GunUpgrades
    {
        get { return gunUpgrades; }
        set
        {
            gunUpgrades = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        redFlashImage = GameObject.Find("Red Flash");
    }
    private void OnEnable()
    {
        if (GamePlayController.Instance != null)
        {
            GamePlayController.OnGameStateChange += GameStateChangeHandle;
        }
        else
        {
            Debug.Log("GamePlayController instance is null");
        }
    }
    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }
    private void Start()
    {
        StopShootingClip();
        audioSource.loop = true;
        GameUIController.instance.SetPlayerStatus();
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (GamePlayController.Instance.state == GameState.PLAY && other.tag == "Enemy")
            {
                if (!collideWithEnemy || playerHasShield) return;
                {
                    redFlashImage.GetComponent<RedFlashAnim>().Flash();
                    PlayerDeath();
                }
            }
        }
    }

    public void StopShootingClip()
    {
        audioSource.Stop();
    }

    private void PlayerDeath()
    {
        GamePlayController.Instance.UpdateState(GameState.DEFEAT);
        AudioController.Instance.PlayAudio(AudioType.PlayerDeath);
    }
    void GameStateChangeHandle(GameState state)
    {
        anim.enabled = (state != GameState.PLAY);

        if (state == GameState.PLAY)
        {
            //gameObject.transform.localScale = new Vector3(1, 1, 1);
            audioSource.Play();
            engineFlames.Play();
            shootingFlames.Play();
        }
        if (state == GameState.INIT)
        {
            gunUpgrades = 1;
            PlayerWeaponBulletsAbstract[] guns = GetComponents<PlayerWeaponBulletsAbstract>();
            foreach (var gun in guns)
            {
                gun.GunUpgrades = gunUpgrades;
            }

        }
    }
    public void FireRockets()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject rocket = Instantiate(rocketPrefab, firePos.position, transform.rotation);
                rocket.transform.Rotate(0, 0, arcAngle - i * 15);
                AudioController.Instance.PlayAudio(AudioType.PalyerShootRockets);
                Targets = GameObject.FindGameObjectsWithTag("Enemy");
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
                Destroy(shields);
            }
            co = StartCoroutine(ShieldsCountDown());
        }
    }

    public void UpgradeGun()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            if (gunUpgrades < 9)
            {
                gunUpgrades++;
                PlayerWeaponBulletsAbstract[] guns = GetComponents<PlayerWeaponBulletsAbstract>();
                foreach (var gun in guns)
                {
                    gun.GunUpgrades = gunUpgrades;
                }
                GameUIController.instance.UpdateRankStatus();
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        if (!playerHasShield)
        {
            Health -= damage;
            GameUIController.instance.UpdatePlayerHealthUI();
            redFlashImage.GetComponent<RedFlashAnim>().Flash();
            if (Health <= 0)
            {
                PlayerDeath();
            }
        }

    }
    public void PlayerAnimation()
    {
        //Speed Level
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
        shields = Instantiate(shieldsVFX, transform.position, Quaternion.identity);
        shields.transform.SetParent(gameObject.transform);
        AudioController.Instance.PlayAudio(AudioType.PlayerShields);
        yield return new WaitForSeconds(4);
        playerHasShield = false;
        Destroy(shields);
    }
}

