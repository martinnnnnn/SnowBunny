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
    public GameObject signXBOX;
    public GameObject signKB;
    private GameObject currentSign;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inputAdapter = FindObjectOfType<InputAdapter>();
        originalParent = transform.parent;
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
            signXBOX.SetActive(false);
            signKB.SetActive(false);
        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, pickupRadius);
    }
}
