using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int chanceToFireAtPlayer = 10;


    private void Start()
    {
        if (Random.Range(1, 100) <= chanceToFireAtPlayer)
        {
            if (Player.Instance != null)
            {
                Vector3 target = Player.Instance.transform.position;
                Vector3 dir = target - transform.position;

                transform.rotation = Quaternion.FromToRotation(Vector3.down, dir);
            }
        }
    }
    void Update()
    {
         transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

}
