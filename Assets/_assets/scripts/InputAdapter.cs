using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputAdapter : MonoBehaviour
{
    public enum InputType
    {
        CONTROLLER_XBOX,
        CONTROLLER_PS4,
        KEYBOARD
    }

    public enum InputKey
    {
        A,
        B,
        TRIGGER
    }

    [HideInInspector] public InputType inputType;


    public Vector3 GetVelocity(Vector3 previousVelocity, float speed)
    {
        Vector3 retval = Vector3.zero;

        switch (inputType)
        {
            case InputType.CONTROLLER_XBOX:
                retval = GetVelocityXBox(previousVelocity, speed);
                break;
            case InputType.CONTROLLER_PS4:
                break;
            case InputType.KEYBOARD:
                retval = GetVelocityKeyBoard(previousVelocity, speed);
                break;
        }

        return retval;
    }

    public bool GetInputDown(InputKey key)
    {
        bool result = false;
        switch (inputType)
        {
            case InputType.CONTROLLER_XBOX:
                result = GetInputDownXBox(key);
                break;
            case InputType.CONTROLLER_PS4:
                break;
            case InputType.KEYBOARD:
                result = GetInputDownKeyBoard(key);
                break;
        }
        return result;
    }

    private bool GetInputDownKeyBoard(InputKey key)
    {
        bool result = false;
        switch (key)
        {
            case InputKey.A:
                result = Input.GetKeyDown(KeyCode.E);
                break;
            case InputKey.B:
                result = Input.GetKeyDown(KeyCode.F);
                break;
            case InputKey.TRIGGER:
                result = Input.GetKeyDown(KeyCode.Space);
                break;
        }
        return result;
    }

    private bool GetInputDownXBox(InputKey key)
    {
        bool result = false;
        switch (key)
        {
            case InputKey.A:
                result = Input.GetButtonDown("A");
                break;
            case InputKey.B:
                result = Input.GetButtonDown("B");
                break;
            case InputKey.TRIGGER:
                result = Input.GetButtonDown("Bump");
                break;
        }
        return result;
    }

    public bool GetInput(InputKey key)
    {
        bool result = false;
        switch (inputType)
        {
            case InputType.CONTROLLER_XBOX:
                result = GetInputXBox(key);
                break;
            case InputType.CONTROLLER_PS4:
                break;
            case InputType.KEYBOARD:
                result = GetInputKeyBoard(key);
                break;
        }
        return result;
    }

    private bool GetInputKeyBoard(InputKey key)
    {
        bool result = false;
        switch (key)
        {
            case InputKey.A:
                result = Input.GetKey(KeyCode.E);
                break;
            case InputKey.B:
                result = Input.GetKey(KeyCode.F);
                break;
            case InputKey.TRIGGER:
                result = Input.GetKey(KeyCode.Space);
                break;
        }
        return result;
    }

    private bool GetInputXBox(InputKey key)
    {
        bool result = false;
        switch (key)
        {
            case InputKey.A:
                result = Input.GetButton("A");
                break;
            case InputKey.B:
                result = Input.GetButton("B");
                break;
            case InputKey.TRIGGER:
                result = Input.GetButton("Bump");
                break;
        }
        return result;
    }

    public Vector3 GetVelocityKeyBoard(Vector3 previousVelocity, float speed)
    {
        Vector3 velocity = previousVelocity;

        if (Input.GetKey(KeyCode.Z))
        {
            velocity.z += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            velocity.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += speed * Time.deltaTime;
        }

        return velocity;
    }

    public Vector3 GetVelocityXBox(Vector3 previousVelocity, float speed)
    {
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");

        return previousVelocity + new Vector3(hMove * speed * Time.deltaTime, 0, vMove * speed * Time.deltaTime);
    }

    private void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                inputType = InputType.CONTROLLER_PS4;
            }
            if (names[x].Length == 33)
            {
                inputType = InputType.CONTROLLER_XBOX;
            }
            else
            {
                inputType = InputType.KEYBOARD;
            }
        }
    }
}