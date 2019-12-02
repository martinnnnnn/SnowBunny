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
    public GameObject sign;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inputAdapter = FindObjectOfType<InputAdapter>();
        sign.SetActive(false);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < pickupRadius)
        {
            sign.SetActive(true);
            sign.transform.LookAt(Camera.main.transform);
            if (inputAdapter.GetInputDown(InputAdapter.InputKey.A))
            {
                player.Eat(this);
                Destroy(gameObject);
            }
        }
        else
        {
            sign.SetActive(false);
        }

        if (inputAdapter.GetInputDown(InputAdapter.InputKey.A) && distanceToPlayer < pickupRadius)
        {
            player.Eat(this);
            Destroy(gameObject);
        }
    }
}
