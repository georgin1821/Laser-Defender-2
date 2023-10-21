using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPathfinding : MonoBehaviour
{
    public event Action OnPositionForamtion;
    public EnemyState enemyState = EnemyState.idle;
    EnemyState currentState;
    int index = 0;
    bool isMoving;
    Vector3 dir;
    Quaternion rot;
    Vector3 formPosition;
    float speed;
    float speedRF = 0.5f;
    bool isMovingHorizontal;
    bool isMovingVertical;
    int AIchanceToReact;
    float aiSpeed;
    float disToPlayer = 2f;
    float rotationSpeed;
    Vector3 startingPosition;
    bool isReacting;
    bool isRotating;
    bool endlessMove;
    bool isFormMoving;
    bool smoothMovement;
    bool isSwampForm;
    float smoothDelta;
    float frequency;
    float magnitude;
    bool isChasingPlayer;
    List<Transform> waypoints;
    int id;
    AnimationCurve curve;

    Transform endTrans;
    DivisionSpawn divSpawn;
    Coroutine co1;

    private void Awake()
    {
        DivisionSpawn.OnLastEnemyAtFormation += OnLastEnemyAtFormationHandler;
    }
    private void OnDestroy()
    {
        DivisionSpawn.OnLastEnemyAtFormation -= OnLastEnemyAtFormationHandler;
    }

    private void OnLastEnemyAtFormationHandler()
    {
        StartCoroutine(StateMachine());
    }

    public void StartingState(EnemyStartingState newState)
    {
        switch (newState)
        {
            case EnemyStartingState.PointToPoint:
                this.endTrans = divSpawn.formationWaypoints[id];
                StartCoroutine(MoveToWaypoint(endTrans.position, false, true, speed));
                break;
            case EnemyStartingState.DivisionToPath:
                waypoints = divSpawn.waypoints;
                StartCoroutine(Deployment());
                break;
            case EnemyStartingState.ChasingPlayer:
                StopAllCoroutines();
                StartCoroutine(ChasingPlayer());
                break;
        }
    }

    private void UpdateState(EnemyState state)
    {
        EndState();
        enemyState = state;
        switch (state)
        {
            case EnemyState.FormationMove:
                break;
            case EnemyState.Formation:
                if (isFormMoving)
                {
                    co1 = StartCoroutine(FormationMove());
                }
                if (isSwampForm)
                {
                    StartCoroutine(SwampForm());
                }
                if (AIchanceToReact > 0)
                {
                    StartCoroutine(AgentChanceToReact());
                }
                break;
            case EnemyState.AgentCycle:
                StopAllCoroutines();
                StartCoroutine(AgentCycle());
                break;
            case EnemyState.ChasingPlayer:
                break;
        }
        currentState = state;
    }

    private void EndState()
    {
        switch (currentState)
        {
            case EnemyState.FormationMove:
                StopCoroutine(co1);
                break;
            case EnemyState.Formation:
                break;
            case EnemyState.AgentCycle:
                break;
            case EnemyState.ChasingPlayer:
                break;
        }
    }
    IEnumerator Deployment()
    {
        while (index < waypoints.Count - 1)
        {
            Vector3 nextPos = waypoints[this.index + 1].position;

            yield return MoveToWaypoint(nextPos, isRotating, smoothMovement, speed);
            index++;
            if (index == waypoints.Count - 1 && endlessMove)
            {
                index = -1;
            }
        }
        //form
        formPosition = divSpawn.formationWaypoints[id].position;
        yield return new WaitForSeconds(0f);
        yield return MoveToWaypoint(formPosition, isRotating, false, speed);
        yield return RotateToAngle(180);
        yield return new WaitForSeconds(.5f);

        OnPositionForamtion?.Invoke();
        //if (isFormMoving)
        //{
        //    UpdateState(EnemyState.FormationMove);
        //}
        if (AIchanceToReact > 0) StartCoroutine(AgentChanceToReact());

    }

    IEnumerator StateMachine()
    {
        if (isFormMoving)
        {
            co1 = StartCoroutine(FormationMove());
        }
        yield return new WaitForSeconds(1f);

        if (isSwampForm)
        {
            StartCoroutine(SwampForm());
        }
        if (AIchanceToReact > 0) StartCoroutine(AgentChanceToReact());
    }
    IEnumerator SwampForm()
    {
        while (true)
        {
            int count = divSpawn.formationWaypoints.Count;
            if (id < count / 2)
            {
                StopCoroutine(co1);
                yield return MoveToWaypoint(divSpawn.formationWaypoints[id + count / 2].position, false, false, speed);
                co1 = StartCoroutine(FormationMove());
                id = id + count / 2;
            }
            else
            {
                if (id >= count / 2)
                {
                    StopCoroutine(co1);
                    yield return MoveToWaypoint(divSpawn.formationWaypoints[id - count / 2].position, false, false, speed);
                    co1 = StartCoroutine(FormationMove());
                    id = id - count / 2;
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator MoveToWaypoint(Vector3 endPos, bool isRotating, bool smoothMovement, float speed, float endPosDistance = 0.1f)
    {
        isMoving = true;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, endPos) >= endPosDistance)
        {
            if (isRotating)
            {
                dir = (endPos - transform.position).normalized;
                rot = Quaternion.LookRotation(Vector3.forward, dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
                if (smoothMovement)
                {
                    transform.position = Vector3.SmoothDamp(transform.position,
                                         endPos, ref velocity, smoothDelta, speed);
                }
                else
                {
                    transform.Translate(0, speed * Time.deltaTime, 0);
                }
            }
            else
            {
                if (!smoothMovement)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                    endPos,
                    speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position,
                                         endPos, ref velocity, smoothDelta, speed);
                }
            }
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator RotateToAngle(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        while (rotation != transform.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 400 * Time.deltaTime);
            yield return null;
        }

    }
    IEnumerator FormationMove()
    {
        startingPosition = gameObject.transform.position;
        Vector3 dir = Vector3.zero;
        float time = .5f;
        float t1 = 0.5f;
        float t2 = 0.5f;
        while (true)
        {
            if (isMovingHorizontal)
            {
                //dir.x = Mathf.Sin(Time.time * frequency - 1f * magnitude;
                dir.x = curve.Evaluate(t1);
                t1 += Time.deltaTime * frequency;
            }
            if (isMovingVertical)
            {
                // dir.y = Mathf.Sin(Time.time * frequency - 0.5f) * magnitude;
                dir.y = curve.Evaluate(t2);
                t2 += Time.deltaTime * (frequency + .7f);
            }
            dir = new Vector3(dir.x * (magnitude), dir.y * (magnitude - .0f), 0);
            time += Time.deltaTime * frequency;
            transform.position = (startingPosition + dir);
            yield return null;
        }
    }
    IEnumerator AgentCycle()
    {
        float distance;

        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        speedRF = aiSpeed * speedRF;
        aiSpeed = aiSpeed * (1 + UnityEngine.Random.Range(-speedRF / 2f, speedRF / 2f));

        yield return MoveToWaypoint(Player.instance.gameObject.transform.position, false, true, aiSpeed, disToPlayer);


        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<EnemyWeaponAbstract>().Firing();
        yield return new WaitForSeconds(1f);


        yield return MoveToWaypoint(startingPosition, false, true, aiSpeed, 0.1f);


        isReacting = false;
        UpdateState(EnemyState.Formation);
    }
    IEnumerator AgentChanceToReact()
    {
        while (!isReacting)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));

            if (UnityEngine.Random.Range(1, 100) <= AIchanceToReact)
            {
                if (isChasingPlayer)
                {
                    StartingState(EnemyStartingState.ChasingPlayer);
                }
                else
                {
                    UpdateState(EnemyState.AgentCycle);
                }
                isReacting = true;
            }
        }
    }
    IEnumerator ChasingPlayer()
    {
        yield return new WaitForSeconds(2f);
        Vector3 velicity = Vector3.zero;
        float distance;
        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        GameObject player = Player.instance.gameObject;
        while (distance > .2f)
        {
            dir = (player.transform.position - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 250 * Time.deltaTime);

            distance = Vector2.Distance(player.transform.position, transform.position);

            transform.position = Vector3.SmoothDamp(transform.position,
                player.transform.position, ref velicity,
                0.9f, speed);
            yield return null;
        }
    }

    public void SetDivisionConfiguration(DivisionConfiguration divConfig, DivisionSpawn spawn, int id = 0)
    {
        divSpawn = spawn;
        speed = divConfig.spawns.moveSpeed;

        this.id = id;
        isFormMoving = divConfig.general.isFormMoving;
        isMovingHorizontal = divConfig.formMove.isMovingHorizontal;
        curve = divConfig.formMove.curve;
        isMovingVertical = divConfig.formMove.isMovingVertical;
        isChasingPlayer = divConfig.general.isChasingPlayer;
        frequency = divConfig.formMove.frequency;
        magnitude = divConfig.formMove.magitude;
        if (divConfig.aISettings != null)
        {
            AIchanceToReact = divConfig.aISettings.AiChanceToReact;
            aiSpeed = divConfig.aISettings.aiSpeed;
        }
        endlessMove = divConfig.general.endlessMove;
        smoothDelta = divConfig.smooth.smoothDelta;
        smoothMovement = divConfig.smooth.smoothMovement;
        rotationSpeed = divConfig.rotation.rotationSpeed;
        isRotating = divConfig.general.isRotating;
        isSwampForm = divConfig.general.isSwampForm;
    }
}

public enum EnemyStartingState
{
    PointToPoint,
    DivisionToPath,
    ChasingPlayer
}
public enum EnemyState
{
    idle,
    FormationMove,
    Formation,
    AgentCycle,
    ChasingPlayer
}

