using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Straw : MonoBehaviour
{
    public float pickupRadius = 3.0f;

    Player player;
    InputAdapter inputAdapter;
    bool pickedUp = false;
    Transform originalParent;

    [Header("Sign")]
    public GameObject sign;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inputAdapter = FindObjectOfType<InputAdapter>();
        originalParent = transform.parent;
        sign.SetActive(false);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < pickupRadius)
        {
            sign.SetActive(true);
            sign.transform.LookAt(Camera.main.transform);

            if (inputAdapter.GetInputDown(InputAdapter.InputKey.A) && !pickedUp)
            {
                pickedUp = true;
                transform.SetParent(player.mouth);
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;

                player.PickupStraw(this, () =>
                {
                    if (pickedUp)
                    {
                        pickedUp = false;
                        transform.SetParent(originalParent);
                        transform.localPosition = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
                        transform.localEulerAngles = Vector3.zero;
                    }
                });
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
        Gizmos.DrawSphere(transform.position, pickupRadius);
    }
}
