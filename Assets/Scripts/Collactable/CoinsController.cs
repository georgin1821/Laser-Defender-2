using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : SimpleSingleton<CoinsController> 
{

    [SerializeField] GameObject coinPrefab;

    float rfPosX = 0.5f;
    public void DropGold(Transform trans, bool multipleCoins, int coins)
    {
        if (multipleCoins)
        {
            coins = Random.Range(coins - 1, coins + 2);

            for (int i = 0; i < coins; i++)
            {
                float posX = Random.Range(trans.position.x - rfPosX, trans.position.x + rfPosX);
                Vector3 temp = trans.position;
                temp.x = posX;

                Instantiate(coinPrefab, temp, Quaternion.identity);
            }
        }
        else
        {
            Instantiate(coinPrefab, trans.position, Quaternion.identity);
        }
    }
}
