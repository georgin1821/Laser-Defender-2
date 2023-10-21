using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserLong : MonoBehaviour
{
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointRight;
    [SerializeField] float timeInShooting = 5;
    [SerializeField] LayerMask shootable;
    [SerializeField] LineRenderer lineRendererLeft;
    [SerializeField] LineRenderer lineRendererRight;
    [SerializeField] float damage = 20f;
    [SerializeField] GameObject impactVFX;

    private int laserRange = 13;

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

    private void Start()
    {
        Invoke("StartShooting", 3f);
    }
    private void StartShooting()
    {
        StartCoroutine(LaserShooting());
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
            yield return new WaitForSeconds(1);
        }
    }
}
