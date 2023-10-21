using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFindVertical : MonoBehaviour
{
    float speed;
    float speedRF;
    void Start()
    {
        StartCoroutine(VerticalMovement());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator VerticalMovement()
    {
        speed = Random.Range(speed - speedRF, speed + speedRF);

        while (gameObject.transform.position.y > -8)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
        EnemyCount.instance.Count--;
    }
    public void SetDivisionConfiguration(DivisionVerticalSpawn config)
    {
        speed = config.speed;
        speedRF = config.speedRF;
    }
}
