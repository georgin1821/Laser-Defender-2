using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float msBetweenShots;
    [SerializeField] float projectileVelocity;

    [SerializeField] bool isShooting = false;

    private float nextShotTime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
        if (GamePlayController.Instance.state == GameState.PLAY)
        {

            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                firePoint.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);

                Instantiate(bullet, firePoint.position, Quaternion.identity);
            }
        }

        }

    }
}
