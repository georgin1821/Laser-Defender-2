using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : SimpleSingleton<Player>, IDamageable
{
    [Header("GameDev Settings")]
    public bool isAlwaysShooting = true;
    [SerializeField] private bool collideWithEnemy = true;
    public bool isAbleToFire = true;

    [Space(10)]
    [Header("VFX")]
    [SerializeField] private ParticleSystem engineVFX;
    [SerializeField] private ParticleSystem shootingFlames;

    [Tooltip("Shooting")]
    public Transform firePos;
    private float arcAngle = 40;

    [Space(5)]
    [Header("Powers Up Weapons")]
    [SerializeField] private GameObject rocketPrefab;
    private int rocketsToShoot = 7;

    [Space(5)]
    public GameObject[] targets; //to  be hide from inspector

    private AudioSource audioSource;
    private Animator anim;
    private bool playerHasShield;
    private Coroutine co;
    private IPlayerWeapon[] weapons;
    private int gunUpgrades = 1;
    private int shieldsDuration = 5;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        weapons = GetComponents<IPlayerWeapon>();
    }

    private void OnEnable()
    {
        // runs after Awake method so runs after singleton initialization
        if (GamePlayController.IsInitialized)
        {
            GamePlayController.OnGameStateChange += GameStateChangeHandle;
        }
        else
        {
            Debug.Log("GamePlayController instance is null");
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }

    private void Start()
    {
        StopShootingClip();
        audioSource.loop = true; // plays the continuous shooting sound
        targets = new GameObject[rocketsToShoot]; // every rocket has a target
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
    }

    private void GameStateChangeHandle(GameState state)
    {
        if (state == GameState.INIT)
        {
            foreach (var weapon in weapons)
            {
                weapon.WeaponUpgrades = 1;
            }
        }
        if (state == GameState.PLAY)
        {
            audioSource.Play();
            engineVFX.Play();
            shootingFlames.Play();
        }
    }

    public void FireRockets()
    {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < rocketsToShoot; i++)
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
        transform.position = new Vector3(0, -6, 0);
        anim.Play("Intro");
    }

    private IEnumerator ShieldsCountDown()
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