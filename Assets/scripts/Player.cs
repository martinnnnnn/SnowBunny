using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody body;
    Animator animator;
    [HideInInspector] public bool isHidding = false;
    BirdController bird;

    [Header("Walk")]
    public float walkSpeed = 0.3f;
    public float walkMaxspeed = 3.0f;

    [Header("Sprint")]
    public float sprintMaxTime;
    public float sprintRecoverRate;
    public float sprintSpeed = 1.0f;
    public float sprintMaxSpeed = 5.0f;
    private float sprintTimeLeft;

    [Header("Food")]
    public Transform mouth;
    public int life;
    public int starveRate;
    public float currentLife;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        bird = FindObjectOfType<BirdController>();
        sprintTimeLeft = sprintMaxTime;
        currentLife = life;
    }

    private void Update()
    {
        currentLife -= starveRate * Time.deltaTime;
        Debug.Log("current life : " + currentLife);
        if (currentLife <= 0)
        {
            Debug.Log("Game Over");
        }

        if (!isHidding)
        {
            HandleMovement();
        }
        else
        {
            HandleHidding();
        }

    }

    Action onLeaveHiddingSpot;
    public void EnterHiddingSpot(Action callback)
    {
        isHidding = true;
        animator.gameObject.SetActive(false);
        onLeaveHiddingSpot = callback;
    }

    Action onSprint;
    public void PickupCarrot(Action callback)
    {
        onSprint = callback;
    }

    private void HandleHidding()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.gameObject.SetActive(true);
            isHidding = false;
            onLeaveHiddingSpot?.Invoke();
        }
    }

    float animationWalkSpeed;
    float animationRunSpeed = 1.5f;
    float animationCurrentSpeed = 2.0f;
    private void HandleMovement()
    {
        float currentSpeed = walkSpeed;
        float currentMaxSpeed = walkMaxspeed;

        animationCurrentSpeed = animationWalkSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            if (sprintTimeLeft >= 0)
            {
                sprintTimeLeft -= Time.deltaTime;
                currentSpeed = sprintSpeed;
                currentMaxSpeed = sprintMaxSpeed;
                onSprint?.Invoke();
                animationCurrentSpeed = animationRunSpeed;
            }
        }
        else
        {
            sprintTimeLeft = Mathf.Min(sprintTimeLeft + sprintRecoverRate * Time.deltaTime, sprintMaxTime);
        }

        Vector3 newVel = body.velocity;

        if (Input.GetKey(KeyCode.Z))
        {
            newVel.z += currentSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newVel.z -= currentSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newVel.x -= currentSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newVel.x += currentSpeed * Time.deltaTime;
        }


        newVel.x = Mathf.Clamp(newVel.x, -currentMaxSpeed, currentMaxSpeed);
        newVel.z = Mathf.Clamp(newVel.z, -currentMaxSpeed, currentMaxSpeed);

        animator.SetFloat("speed", newVel.magnitude);
        //if (newVel.magnitude > 0.1f)
        //{
        //    animator.speed = animationCurrentSpeed;
        //}

        body.velocity = newVel;

        if (body.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(body.velocity, Vector3.up);
        }
    }

    public void Eat(Grass grass)
    {
        currentLife = Mathf.Min(currentLife + grass.foodValue, life);
    }
}