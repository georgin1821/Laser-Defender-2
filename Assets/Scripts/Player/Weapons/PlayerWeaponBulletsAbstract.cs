using System.Collections;
using System.Threading;
using UnityEngine;



/* Abstract class for weapon type Bullets
 * subclasses are BulletsWeaponAngle BulletsStraightWeapon
  */



public abstract class PlayerWeaponBulletsAbstract : IPlayerWeapon
{

    protected Transform firePoint;
    protected bool isAlwaysShooting;
    protected float nextShotTime = 0;
    [SerializeField] protected float busrtShootingTimer;
    [SerializeField] protected float msBetweenShots;
    [SerializeField] protected bool isBurstShooting;

    [SerializeField, Range(0.01f, .3f)] protected float XposOffset = 0.15f;
    [SerializeField, Range(0.01f, .3f)] protected float YposOffset = 0.15f;

    [SerializeField] protected PlayerProjectile projectile;
    public int bulletVelocity;

    protected abstract void InstatiateProjectiles(int upgrades);
    protected virtual void Shooting()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;

            InstatiateProjectiles(WeaponUpgrades);
        }
    }

    private void OnValidate()
    {
        //change the speed on projectile speed, al parameters in one spotf
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
            yield return new WaitForSeconds(.3f);
            timer = 0;
        }
    }
}


