using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiddingSpot : MonoBehaviour
{
    public float hideRadius = 3.0f;
    public bool isBurrow = false;

    Player player;
    InputAdapter inputAdapter;
    bool occupied = false;

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

        if (distanceToPlayer < hideRadius)
        {
            sign.SetActive(true);
            sign.transform.LookAt(Camera.main.transform);

            if (inputAdapter.GetInputDown(InputAdapter.InputKey.A))
            {
                occupied = true;
                player.EnterHiddingSpot(() =>
                {
                    occupied = false;
                }, isBurrow);
            }
        }
        else
        {
            sign.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, hideRadius);
    }
}
