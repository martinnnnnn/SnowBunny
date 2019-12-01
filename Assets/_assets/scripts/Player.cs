using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Straw")]
    public int strawCountNeeded = 10;
    private int currentSrawCount = 0;

    [Header("Animation")]
    public float animationWalkSpeed = 1.5f;
    public float animationRunSpeed = 2.0f;
    float animationCurrentSpeed;

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
        HandleLife();
        

        if (!isHidding)
        {
            HandleMovement();
        }
        else
        {
            HandleHidding();
        }

    }

    public Image roundProgressionBar;
    private void HandleLife()
    {
        currentLife -= starveRate * Time.deltaTime;
        roundProgressionBar.fillAmount = currentLife / (float)life;
        if (currentLife <= 0)
        {
            EndGameData.result = EndGameData.Result.DEFEAT;
            SceneManager.LoadScene(3);
        }
    }

    Action onLeaveHiddingSpot;
    public void EnterHiddingSpot(Action callback, bool isBurrow)
    {
        isHidding = true;
        animator.gameObject.SetActive(false);
        onLeaveHiddingSpot = callback;
        if (isBurrow && carrot != null)
        {
            currentSrawCount++;
            Debug.Log("current straw count : " + currentSrawCount);
            Destroy(carrot.gameObject);
            onSprint = null;

            if (currentSrawCount == strawCountNeeded)
            {
                EndGameData.result = EndGameData.Result.VICTORY;
                SceneManager.LoadScene(3);
            }
        }
    }

    Action onSprint;
    [HideInInspector] Straw carrot = null;
    public void PickupStraw(Straw carrot, Action callback)
    {
        this.carrot = carrot;
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
                carrot = null;
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

        animationCurrentSpeed = Mathf.Min(animationCurrentSpeed, newVel.magnitude);
        animator.SetFloat("speed", animationCurrentSpeed);

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