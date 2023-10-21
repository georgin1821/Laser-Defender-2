using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillLaser : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] GameObject line;

    void Start()
    {
        StartCoroutine(LaserSkill());
    }

    IEnumerator LaserSkill()
    {
        float range = Random.Range(1f, 3f);
        while (transform.position.y > range)
        {
            yield return null;
        }

        line.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        line.SetActive(false);
        laser.SetActive(true);
        AudioController.Instance.PlayAudio(AudioType.EnemyLaserSkill);

        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        laser.SetActive(false);
        AudioController.Instance.StopAudio(AudioType.EnemyLaserSkill);
    }

}
