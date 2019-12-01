﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        SetupHoverTriggers(backButton);
    }

    void SetupHoverTriggers(Button button)
    {
        var buttonText = button.GetComponentInChildren<TMP_Text>();

        EventTrigger.Entry pointerEnterEvent = new EventTrigger.Entry();
        pointerEnterEvent.eventID = EventTriggerType.PointerEnter;
        pointerEnterEvent.callback.AddListener((eventData) => { buttonText.color = Color.black; });

        EventTrigger.Entry pointerExitEvent = new EventTrigger.Entry();
        pointerExitEvent.eventID = EventTriggerType.PointerExit;
        pointerExitEvent.callback.AddListener((eventData) => { buttonText.color = Color.white; });

        var trigger = button.gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(pointerEnterEvent);
        trigger.triggers.Add(pointerExitEvent);
    }

}
