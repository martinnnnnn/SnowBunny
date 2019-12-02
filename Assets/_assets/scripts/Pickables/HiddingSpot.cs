using DG.Tweening;
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

        if (distanceToPlayer < hideRadius)
        {
            currentSign.SetActive(true);
            currentSign.transform.LookAt(-Camera.main.transform.position);

            if (inputAdapter.GetInputDown(InputAdapter.InputKey.A) && !occupied)
            {
                Debug.Log("hello");
                occupied = true;
                player.EnterHiddingSpot(() =>
                {
                    Sequence sec = DOTween.Sequence();
                    sec.AppendInterval(0.1f)
                       .OnComplete(() =>
                       {
                           occupied = false;
                       });
                }, isBurrow);
            }
        }
        else
        {
            currentSign.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, hideRadius);
    }
}
