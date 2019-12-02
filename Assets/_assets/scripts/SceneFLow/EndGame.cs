using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    
    public Button backButton;
    public TMP_Text resultText;
    InputAdapter inputAdapter;

    private void Start()
    {
        inputAdapter = GetComponent<InputAdapter>();

        switch (EndGameData.result)
        {
            case EndGameData.Result.VICTORY:
                resultText.text = "Victory!";
                break;
            case EndGameData.Result.DEFEAT:
                resultText.text = "Defeat!";
                break;
        }

        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        SetupHoverTriggers(backButton);
    }

    private void Update()
    {
        if (inputAdapter.GetInputDown(InputAdapter.InputKey.B))
        {
            SceneManager.LoadScene(0);
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
