using System.Collections;
using System.Threading;
using UnityEngine;



/* Abstract class for weapon type Bullets
 * subclasses are BulletsWeaponAngle BulletsStraightWeapon
  */



public abstract class PlayerBulletsAbstract : IPlayerWeapon
{

    protected Transform firePoint;
    protected bool isAlwaysShooting;
    private float nextShotTime = 0;

    [Header("Shooting")]
    [SerializeField] protected float msBetweenShots;
    [SerializeField][Range(1, 50)] private int bulletVelocity;
    [SerializeField] protected bool isBurstShooting;
    [SerializeField] protected float busrtShootingTimer;
    [SerializeField][Range(.1f, 2f)] protected float busrtShootingCooldown;

    [Header("Bullets Offset")]
    [SerializeField, Range(0.01f, .3f)] protected float XposOffset = 0.15f;
    [SerializeField, Range(0.01f, .3f)] protected float YposOffset = 0.15f;

    [Header("Prefab")]
    [SerializeField] protected PlayerProjectile projectile;


    private void OnValidate()
    {
        //change the speed on projectile speed, al parameters in one spot
        projectile.speed = bulletVelocity;

    }
    private void Start()
    {
        firePoint = Player.Instance.firePos;
        isAlwaysShooting = Player.Instance.isAlwaysShooting;
        if (isBurstShooting)
        {
            StartCoroutine(BurstShooting());
        }

    }
    private void Update()
    {
        if (GamePlayController.Instance.state != GameState.PLAY || isBurstShooting || !Player.Instance.isAbleToFire)
        {
            return;
        }
        //Game Dev Setting
        if (isAlwaysShooting)
        {
            Shooting();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shooting();
            }
        }
    }

    protected abstract void InstatiateProjectiles(int upgrades);
    protected virtual void Shooting()
    {
        //Shooting at steady interval (msBetweenShots)
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            InstatiateProjectiles(WeaponUpgrades);
        }
    }

    private IEnumerator BurstShooting()
    {
        float timer = 0;
        while (true)
        {
            while (timer < busrtShootingTimer)
            {
                if (GamePlayController.Instance.state == GameState.PLAY)
                {
                    Shooting();
                    timer += Time.deltaTime;
                }
                yield return null;
            }
            yield return new WaitForSeconds(busrtShootingCooldown);
            timer = 0;
        }
    }
}



