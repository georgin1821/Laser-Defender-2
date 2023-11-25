using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletsStraight : PlayerBulletsAbstract
{

    protected override void InstatiateProjectiles(int upgrades)
    {
        float offset1 = 0;
        float offset2 = 0;

        offset1 = XposOffset * (upgrades - 1) / 2;

        for (int i = 0; i < upgrades; i++)
        {
            {
                if (upgrades == 2)
                {
                    offset2 = 0;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        offset2 = -YposOffset;
                    }

                    else
                    {
                        offset2 = YposOffset;
                    }

                }
            }
                PlayerProjectile progectile = Instantiate(projectile, firePoint.position - new Vector3(offset1, offset2, 0), Quaternion.identity);
                offset1 = offset1 - XposOffset;
            }
        }

    }
