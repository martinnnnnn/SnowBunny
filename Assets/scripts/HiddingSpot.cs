﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiddingSpot : MonoBehaviour
{
    public float hideRadius = 3.0f;

    Player player;
    bool occupied = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();

    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (Input.GetKeyDown(KeyCode.E) && distanceToPlayer < hideRadius)
        {
            occupied = true;
            player.EnterHiddingSpot(() =>
            {
                occupied = false;
            });
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == player.gameObject && !occupied)
    //    {
    //        occupied = true;
    //        player.EnterHiddingSpot(() =>
    //        {
    //            occupied = false;
    //        });
    //    }
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, hideRadius);
    }
}
