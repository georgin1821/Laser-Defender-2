using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    [SerializeField] GameObject path1, path2;
   public float speed, rotationSpeed;

    List<Transform> routeTrans1, routeTrans2;
    public float dis;
    FightStage stage;
    float speedRF = 0.6f;

    private void Awake()
    {
        GetWaypoints();
    }
    private void Start()
    {
        UpdateState(FightStage.INTRO);
    }

    void UpdateState(FightStage newStage)
    {
        stage = newStage;
        switch (newStage)
        {
            case FightStage.INTRO:
                StartCoroutine(IntroStateRoutine());
                break;
            case FightStage.PHASE1:
                break;
            case FightStage.PHASE2:
                break;
            case FightStage.DEATH:
                break;
            default:
                break;
        }
    }


    IEnumerator IntroStateRoutine()
    {
        AudioController.Instance.LoopAudio(AudioType.bossMoving);

        int index = 0;
        Vector3 velocity = Vector3.zero;
           speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));
        while (index < routeTrans1.Count - 1)
        {
            Vector3 nextPos = routeTrans1[index + 1].position;

            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity , 0.8f, speed);
            dis = Vector3.Distance(nextPos, transform.position);
            if (Vector3.Distance(nextPos, transform.position) < 0.1f)
            {
                index++;
               speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));

            }
            if(index == routeTrans1.Count - 1)
            {
                index = -1;
            }
            yield return null;
        }
    }
    private void GetWaypoints()
    {
        routeTrans1 = new List<Transform>();
        routeTrans2 = new List<Transform>();

        foreach (Transform child in path1.transform)
        {
            routeTrans1.Add(child);
        }
        foreach (Transform child in path2.transform)
        {
            routeTrans2.Add(child);
        }
    }
    //public void SetConfig(DivisionConfiguration dv)
    //{
    //    speed = dv.moveSpeed;
    //}
}
enum FightStage
    {
        INTRO,
        PHASE1,
        PHASE2,
        DEATH
    }
