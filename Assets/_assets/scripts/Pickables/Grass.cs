using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class Grass : MonoBehaviour
{
    public float pickupRadius = 2.5f;
    public int foodValue = 25;

    Player player;
    InputAdapter inputAdapter;

    [Header("Sign")]
    public GameObject signXBOX;
    public GameObject signKB;
    private GameObject currentSign;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inputAdapter = FindObjectOfType<InputAdapter>();
        signXBOX.SetActive(false);
        signKB.SetActive(false);
    }

    private void Update()
    {
        currentSign = inputAdapter.inputType == InputAdapter.InputType.CONTROLLER_XBOX ? signXBOX : signKB;

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < pickupRadius)
        {
            currentSign.SetActive(true);
            currentSign.transform.LookAt(Camera.main.transform.position);
            if (inputAdapter.GetInputDown(InputAdapter.InputKey.A))
            {
                player.Eat(this);
                Destroy(gameObject);
            }
        }
        else
        {
            signXBOX.SetActive(false);
            signKB.SetActive(false);
        }

        if (inputAdapter.GetInputDown(InputAdapter.InputKey.A) && distanceToPlayer < pickupRadius)
        {
            player.Eat(this);
            Destroy(gameObject);
        }
    }
}
