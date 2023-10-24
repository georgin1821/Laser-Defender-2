using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] float forwardSpeed;
    [SerializeField] float playerRange;
    [SerializeField] float smoothTime;
    public float acelaration;

    private Vector3 velocity = Vector3.zero;
    private Animator anim;
    private float rfSpeed = .2f;

    private float speed = -1f;
    private float accelaration = 1.5f;

    private void Start()
    {
        forwardSpeed = Random.Range(forwardSpeed - rfSpeed, forwardSpeed + rfSpeed);
    }
    void Update()
    {
        {
            if (Vector3.Distance(Player.Instance.transform.position, this.transform.position) > playerRange)
            {
                speed = Mathf.Clamp(speed + (accelaration * Time.deltaTime), -1f, 5f);
                transform.Translate(new Vector3(0, -forwardSpeed, 0) * speed * Time.deltaTime);
            }
            else
            {
                Vector3 target = Player.Instance.transform.position;
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, 15);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            AudioController.Instance.PlayAudio(AudioType.CollectGold);
            int currentCoins = GameDataManager.Instance.coins;
            GameDataManager.Instance.coins += 100;
            GamePlayController.Instance.levelCoins += 100;
            GameUIController.Instance.UpdateCoins(currentCoins, GameDataManager.Instance.coins);
        }
    }

}
