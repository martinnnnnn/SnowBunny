using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAnimator : MonoBehaviour
{
    public Action OnJumpStart;

    void StartJump()
    {
        OnJumpStart?.Invoke();
    }
}