using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grass : MonoBehaviour
{
    public float pickupRadius = 2.5f;
    public int foodValue = 25;

    Player player;
    InputAdapter inputAdapter;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inputAdapter = FindObjectOfType<InputAdapter>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (inputAdapter.GetInputDown(InputAdapter.InputKey.A) && distanceToPlayer < pickupRadius)
        {
            player.Eat(this);
            Destroy(gameObject);
        }
    }
}
