using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    
    public Button startButton;
    public Button creditsButton;
    InputAdapter inputAdapter;


    private void Start()
    {
        inputAdapter = GetComponent<InputAdapter>();

        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });

        SetupHoverTriggers(startButton);

        creditsButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });

        SetupHoverTriggers(creditsButton);
    }

    private void Update()
    {
        if (inputAdapter.GetInputDown(InputAdapter.InputKey.A))
        {
            SceneManager.LoadScene(2);
        }
        if (inputAdapter.GetInputDown(InputAdapter.InputKey.B))
        {
            SceneManager.LoadScene(1);
        }
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
