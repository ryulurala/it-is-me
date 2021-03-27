﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    Action _inputAction = null;
    float _pressedTime = 0f;
    Vector2 _startPos;

    public void OnAwake()
    {
        if (Util.IsMobile)
        {
            // Mobile
            _inputAction -= OnTouchEvent;
            _inputAction += OnTouchEvent;

            _inputAction -= OnPadEvent;
            _inputAction += OnPadEvent;

            MouseAction = null;
            KeyAction = null;
        }
        else
        {
            // PC
            _inputAction -= OnMouseEvent;
            _inputAction += OnMouseEvent;

            _inputAction -= OnKeyEvent;
            _inputAction += OnKeyEvent;

            TouchAction = null;
            PadAction = null;
        }
    }

    public void OnUpdate()
    {
        // UI Click 상태
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;

        if (_inputAction != null)
            _inputAction.Invoke();
    }

    public void Clear()
    {
        MouseAction = null;
        TouchAction = null;
    }

    #region Mobile
    public Action<Define.TouchEvent> TouchAction = null;
    public Action<Define.PadEvent, Vector3> PadAction = null;

    void OnTouchEvent()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TouchAction.Invoke(Define.TouchEvent.TabWithOneStart);
                _pressedTime = Time.time;   // 시간 측정
                _startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                TouchAction.Invoke(Define.TouchEvent.PressWithOne);
            }
        }
    }

    void OnPadEvent()
    {
        if (GamePad.Pad == null)
            return;

        Vector3 dir = Vector3.zero + new Vector3(GamePad.Pad.Direction.x, 0, GamePad.Pad.Direction.y);
        Debug.Log($"Direction: {GamePad.Pad.Direction}");
        Debug.Log($"dir: {dir}");
        // + -> +
        // - -> -
        // Debug.Log($"Direction: {GamePad.Pad.Direction}");
        // Debug.Log($"dir: {dir}");

        // if (dir.x > 0 && dir.z > 0)
        //     dir += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        // else if (dir.x < 0 && dir.z < 0)
        //     dir += new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z).normalized;
        // else if (dir.x > 0 && dir.z < 0)
        //     dir += new Vector3(Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z).normalized;
        // else if (dir.x < 0 && dir.z > 0)
        //     dir += new Vector3(-Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;


        if (GamePad.Pad.GetPad(GamePad.PadCode.Joystick))
            PadAction.Invoke(Define.PadEvent.Dragging, dir);
        if (GamePad.Pad.GetPad(GamePad.PadCode.ButtonA))
            PadAction.Invoke(Define.PadEvent.AttackButton, dir);
        if (GamePad.Pad.GetPad(GamePad.PadCode.ButtonR))
            PadAction.Invoke(Define.PadEvent.RunButton, dir);
        if (GamePad.Pad.GetPad(GamePad.PadCode.ButtonJ))
            PadAction.Invoke(Define.PadEvent.JumpButton, dir);
    }

    #endregion

    #region PC
    public Action<Define.MouseEvent> MouseAction = null;
    public Action<Define.KeyEvent, Vector3> KeyAction = null;

    void OnMouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
            MouseAction.Invoke(Define.MouseEvent.LeftClick);

        if (Input.GetMouseButtonDown(1))
            MouseAction.Invoke(Define.MouseEvent.RightStart);
        else if (Input.GetMouseButton(1))
            MouseAction.Invoke(Define.MouseEvent.RightPress);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            MouseAction.Invoke(Define.MouseEvent.ScrollWheel);
    }

    void OnKeyEvent()
    {
        // 방향 벡터 축적
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            dir += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        if (Input.GetKey(KeyCode.S))
            dir += new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z).normalized;
        if (Input.GetKey(KeyCode.A))
            dir += new Vector3(-Camera.main.transform.right.x, 0, -Camera.main.transform.right.z).normalized;
        if (Input.GetKey(KeyCode.D))
            dir += new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        // 결정
        if (dir != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                KeyAction.Invoke(Define.KeyEvent.ShiftWASD, dir);
            else
                KeyAction.Invoke(Define.KeyEvent.WASD, dir);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            KeyAction.Invoke(Define.KeyEvent.SpaceBar, dir);

    }
    #endregion
}
