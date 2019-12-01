using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    [Header("Physical")]
    public float speed;

    [Header("Respawn")]
    public float respawnDistance;
    public int respawnDurationMin;
    public int respawnDurationMax;
    private float currentRespawnTime;

    [Header("Catch - Hunt")]
    public float catchTime;
    public float catchDecreaseRate;
    public float distanceToLetGo;
    private float currentCatch;

    [Header("Catch - Wait")]
    public float waitTime;
    private float currentWaitTime;

    Player player;
    MeshRenderer birdRenderer;
    State state = State.INACTIVE_IN;
    State lastState = State.LEAVING_OUT;
    Vector3 leaveDirection = Vector3.zero;
    private Vector3 lastPosition;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        birdRenderer = GetComponent<MeshRenderer>();
        lastPosition = transform.position;
    }

    
    private void Update()
    {
        if (state != lastState)
        {
            //Debug.Log(state);
            lastState = state;
        }

        switch (state)
        {
            case State.INACTIVE_IN:
                currentRespawnTime = Time.time + random.Next(respawnDurationMin, respawnDurationMax);
                transform.GetChild(0).gameObject.SetActive(false);
                state = State.INACTIVE;
                break;

            case State.INACTIVE:
                if (Time.time > currentRespawnTime)
                {
                    state = State.INACTIVE_OUT;
                }
                break;

            case State.INACTIVE_OUT:
                if (player.isHidding)
                {
                    state = State.WAITING_IN;
                }
                else
                {
                    state = State.CHASING_IN;
                }
                break;

            case State.CHASING_IN:
                currentCatch = 0;
                transform.position = RandomPointOnCircleEdge(respawnDistance);
                transform.GetChild(0).gameObject.SetActive(true);
                state = State.CHASING;
                break;

            case State.CHASING:
                if (player.isHidding)
                {
                    state = State.WAITING_IN;
                }
                else
                {
                    float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
                    if (distanceToPlayer > 1)
                    {
                        Vector3 dirNormalized = (player.transform.position - transform.position).normalized;
                        transform.position = transform.position + dirNormalized * speed * Time.deltaTime;
                    }

                    if (distanceToPlayer < 2)
                    {
                        currentCatch += Time.deltaTime;
                        if (currentCatch >= catchTime)
                        {
                            state = State.EATING;
                        }
                    }
                    else
                    {
                        currentCatch = Mathf.Max(currentCatch - catchDecreaseRate * Time.deltaTime, 0);
                    }
                }
                break;

            case State.CHASING_OUT:
                break;

            case State.EATING:
                EndGameData.result = EndGameData.Result.DEFEAT;
                SceneManager.LoadScene(3);
                state = State.LEAVING_IN;
                break;

            case State.WAITING_IN:
                currentWaitTime = waitTime;
                transform.GetChild(0).gameObject.SetActive(true);
                state = State.WAITING;
                break;

            case State.WAITING:
                if (!player.isHidding)
                {
                    state = State.CHASING;
                }
                else
                {
                    currentWaitTime -= Time.deltaTime;
                    if (currentWaitTime >= 0)
                    {
                        transform.RotateAround(player.transform.position, Vector3.up, 20 * Time.deltaTime);
                    }
                    else
                    {
                        state = State.WAITING_OUT;
                    }
                }
                break;

            case State.WAITING_OUT:
                state = State.LEAVING_IN;
                break;

            case State.LEAVING_IN:
                leaveDirection = transform.forward;
                state = State.LEAVING;
                break;

            case State.LEAVING:
                if (Vector3.Distance(transform.position, player.transform.position) < respawnDistance)
                {
                    transform.position = transform.position + leaveDirection * speed * Time.deltaTime;
                }
                else
                {
                    state = State.LEAVING_OUT;
                }
                break;


            case State.LEAVING_OUT:
                state = State.INACTIVE_IN;
                break;
        }

        if (transform.position - lastPosition != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - lastPosition, Vector3.up), 0.05f);
        }
        lastPosition = transform.position;
    }

    System.Random random = new System.Random();

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y) + player.transform.position;
    }

    enum State
    {
        INACTIVE_IN,
        INACTIVE,
        INACTIVE_OUT,
        CHASING_IN,
        CHASING,
        CHASING_OUT,
        EATING,
        WAITING_IN,
        WAITING,
        WAITING_OUT,
        LEAVING_IN,
        LEAVING,
        LEAVING_OUT
    }
}
