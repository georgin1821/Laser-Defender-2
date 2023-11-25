using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserLong : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] LineRenderer lineRendererLeft;
    [SerializeField] LineRenderer lineRendererRight;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointRight;
    [SerializeField] LayerMask shootable;

    [Header("Shooting")]
    [SerializeField][Range(.1f, 3)] float timeInShooting = 5;
    [SerializeField][Range(0, 1)] private float cooldownLaser;
    [SerializeField] float damage = 20f;

    [Header("VFX")]
    [SerializeField] GameObject impactVFX;

    private int laserRange = 13;
    private IEnumerator routine;

    [SerializeField] private int gunUpgrades = 1;
    public int GunUpgrades
    {
        get { return gunUpgrades; }
        set
        {
            gunUpgrades = value;
            if (gunUpgrades > 10) gunUpgrades = 10;
        }
    }
    private void Awake()
    {
        GamePlayController.OnGameStateChange += OnGameStateChangeHandler;
    }

    private void OnGameStateChangeHandler(GameState state)
    {
        switch(state)
        {
            case GameState.PLAY:
                StartTheLaserRoutine();
                break;
             default:
                StopTheLaserRoutine();
                break;
        }
    }

    private void StopTheLaserRoutine()
    {
        if (routine != null) StopCoroutine(routine);
    }

    private void StartTheLaserRoutine()
    {
        StopTheLaserRoutine();
        routine = LaserShooting();
        StartCoroutine(routine);
    }

    IEnumerator LaserShooting()
    {
        float t1 = 0;

        while (true)
        {
            while (t1 < timeInShooting)
            {
                t1 += Time.deltaTime;

                lineRendererLeft.gameObject.SetActive(true);
                lineRendererRight.gameObject.SetActive(true);

                RaycastHit hitleft;
                RaycastHit hitRight;

                lineRendererLeft.SetPosition(0, firePointLeft.position);
                lineRendererRight.SetPosition(0, firePointRight.position);

                lineRendererLeft.SetPosition(1, firePointLeft.position + Vector3.up * laserRange);
                lineRendererRight.SetPosition(1, firePointRight.position + Vector3.up * laserRange);

                bool isHitLeft = Physics.Raycast(firePointLeft.position, Vector3.up, out hitleft, laserRange, shootable);
                if (isHitLeft)
                {
                    if (hitleft.collider.gameObject.TryGetComponent(out Enemy enemy))
                    {
                        enemy.ProccessHitLaser(damage);
                        GameObject explotion = Instantiate(impactVFX, hitleft.point, Quaternion.identity);
                        Destroy(explotion, 1f);
                        lineRendererLeft.SetPosition(1, hitleft.point);
                    }
                }

                bool isHitRight = Physics.Raycast(firePointRight.position, Vector3.up, out hitRight, laserRange, shootable);
                if (isHitRight)
                {
                    if (hitRight.collider.gameObject.TryGetComponent(out Enemy enemy))
                    {
                        enemy.ProccessHitLaser(damage);
                        GameObject explotion = Instantiate(impactVFX, hitRight.point, Quaternion.identity);
                        Destroy(explotion, 1f);
                        lineRendererRight.SetPosition(1, hitRight.point);
                    }
                }
                yield return null;
            }

            lineRendererLeft.gameObject.SetActive(false);
            lineRendererRight.gameObject.SetActive(false);
            t1 = 0;
            yield return new WaitForSeconds(cooldownLaser);
        }
    }
}
