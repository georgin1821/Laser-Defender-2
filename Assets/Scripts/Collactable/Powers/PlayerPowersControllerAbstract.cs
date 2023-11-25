using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPowersControllerAbstract : MonoBehaviour
{
    [SerializeField] float playerRange;
    [SerializeField] float smoothTime;
    [SerializeField] float forwardSpeed;
    [SerializeField] GameObject gainPowerVFX;
    [SerializeField] AudioType collectClip;

    private Vector3 velocity = Vector3.zero;

    protected void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position, this.transform.position) > playerRange)
        {
            transform.Translate(-Vector3.up * forwardSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 target = Player.Instance.transform.position;

            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            AudioController.Instance.PlayAudio(collectClip);
            GameObject vfx = Instantiate(gainPowerVFX, Player.Instance.transform.position, Quaternion.identity);
        }
    }


}
