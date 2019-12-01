using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class EndGameData
{
    public enum Result
    {
        VICTORY,
        DEFEAT
    }
    public static Result result;
}