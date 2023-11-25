using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVortex : MonoBehaviour
{
    public float speed;
    public GameObject path;

    public Transform[] waypoints;

    void Start()
    {
        waypoints = path.GetComponentsInChildren<Transform>();
        StartCoroutine(FollowPath());

    }

    IEnumerator FollowPath()
    {
        int index = 0;
        while (index < waypoints.Length - 1)
        {
            Vector3 nextPos = waypoints[index + 1].position;
            Vector3 dir = (nextPos - transform.position).normalized;
            AudioController.Instance.PlayAudio(AudioType.PlayerShields);

            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }
        AudioController.Instance.StopAudio(AudioType.PlayerShields);
        Destroy(gameObject);

    }
}
