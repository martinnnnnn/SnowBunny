using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody body;
    Animator animator;
    [HideInInspector] public bool isHidding = false;
    BirdController bird;
    InputAdapter inputAdapter;
    SoundManager soundManager;
    PlayerAnimator playerAnimator;

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
    private int currentStrawCount = 0;
    public TMP_Text strawCountText;

    [Header("Animation")]
    public float animationWalkSpeed = 1.5f;
    public float animationRunSpeed = 2.0f;
    float animationCurrentSpeed;

    [Header("FootPrints")]
    public GameObject footPrintsPrefab;
    public Transform footPrintsContainer;
    public float timeBetweenFootPrintsMin = 0.3f;
    public float timeBetweenFootPrintsMax = 0.6f;
    float nextFootPrintsTime = 0.0f;
    public float timeBeforeShrink = 2.0f;
    public float shrinkDuration = 5.0f;

    [Header("HUD")]
    public Image roundProgressionBar;

    [Header("Shadow")]
    public Transform shadow;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerAnimator.OnJumpStart += DoShadowAnim;
        bird = FindObjectOfType<BirdController>();
        inputAdapter = FindObjectOfType<InputAdapter>();
        soundManager = FindObjectOfType<SoundManager>();
        sprintTimeLeft = sprintMaxTime;
        currentLife = life;
        strawCountText.text = "0";
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
            currentStrawCount++;
            strawCountText.text = currentStrawCount.ToString();
            Destroy(carrot.gameObject);
            onSprint = null;

            if (currentStrawCount == strawCountNeeded)
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
        if (inputAdapter.GetInputDown(InputAdapter.InputKey.B))
        {
            animator.gameObject.SetActive(true);
            isHidding = false;
            onLeaveHiddingSpot?.Invoke();
        }
    }

    int playsound;
    private void HandleMovement()
    {
        float currentSpeed = walkSpeed;
        float currentMaxSpeed = walkMaxspeed;

        animationCurrentSpeed = animationWalkSpeed;

        if (inputAdapter.GetInput(InputAdapter.InputKey.TRIGGER))
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

        Vector3 newVel = inputAdapter.GetVelocity(body.velocity, currentSpeed);

        animationCurrentSpeed = Mathf.Min(animationCurrentSpeed, newVel.magnitude);
        animator.SetFloat("speed", animationCurrentSpeed);

        if (animationCurrentSpeed > 0.1f)
        {
            bird.start = true;
        }

        body.velocity = newVel;

        if (body.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(body.velocity, Vector3.up);

            if (nextFootPrintsTime <= Time.time)
            {
                nextFootPrintsTime = Time.time + UnityEngine.Random.Range(timeBetweenFootPrintsMin, timeBetweenFootPrintsMax);
                GameObject newFoorPrint = Instantiate(footPrintsPrefab, footPrintsContainer);
                newFoorPrint.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
                newFoorPrint.transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y + UnityEngine.Random.Range(-20, 20), transform.rotation.eulerAngles.z + UnityEngine.Random.Range(-20, 20));

                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(timeBeforeShrink)
                    .Append(newFoorPrint.transform.DOScale(Vector3.zero, shrinkDuration))
                    .OnComplete(() => Destroy(newFoorPrint));

                playsound++;
                if (playsound%4 == 0)
                {
                    soundManager.PlaySnowSound();
                }
            }
        }
    }

    private void DoShadowAnim()
    {
        //shadow.DOPunchScale(shadow.localScale * 1.05f, 0.17f, elasticity: 0);
    }

    public void Eat(Grass grass)
    {
        currentLife = Mathf.Min(currentLife + grass.foodValue, life);
    }
}