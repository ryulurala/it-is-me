﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public Define.State State
    {
        get { return _state; }
        set
        {
            // 무분별한 State 변경 방지
            if (value == _state)
                return;

            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    // anim.CrossFade("Die", 0.1f);
                    break;
                case Define.State.Idle:
                    anim.CrossFade("Idle", 0.05f);
                    break;
                case Define.State.Walking:
                    anim.CrossFade("Walk", 0.05f);
                    break;
                case Define.State.Running:
                    anim.CrossFade("Run", 0.05f);
                    break;
                case Define.State.Attack:
                    // anim.CrossFade("Attack", 0.1f);
                    break;
            }
        }
    }

    protected override void OnStart()
    {
        // Settings
        State = Define.State.Idle;
        WorldObjectType = Define.WorldObject.Player;

        // Listener
        Manager.Input.MouseAction -= OnMouseEvent;  // Pooling으로 인해 두 번 등록 방지
        Manager.Input.MouseAction += OnMouseEvent;

        Manager.Input.PadAction -= OnPadEvent;      // Pooling으로 인해 두 번 등록 방지
        Manager.Input.PadAction += OnPadEvent;

        Manager.Input.KeyAction -= OnKeyEvent;
        Manager.Input.KeyAction += OnKeyEvent;
    }

    protected override void OnUpdate()
    {
        switch (_state)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Walking:
                UpdateWalking();
                break;
            case Define.State.Running:
                UpdateRunning();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    #region UpdateState
    void UpdateDie() { }
    void UpdateIdle() { }
    void UpdateWalking() { }
    void UpdateRunning() { }
    void UpdateAttack() { }
    #endregion

    #region Mobile
    void OnPadEvent(Define.PadEvent padEvent, Vector3 dir)
    {
        if (_state == Define.State.Die)
            return;

        switch (padEvent)
        {
            case Define.PadEvent.OnIdle:
                State = Define.State.Idle;
                break;
            case Define.PadEvent.OnWalk:
                Move(_walkSpeed, Define.State.Walking, dir);
                break;
            case Define.PadEvent.OnRun:
                Move(_runSpeed, Define.State.Running, dir);
                break;
            case Define.PadEvent.OnAttack:
                Debug.Log("Attack!");
                break;
            case Define.PadEvent.OnJump:
                Debug.Log("Jump!");
                break;
        }
    }
    #endregion

    #region PC
    void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (_state == Define.State.Die)
            return;

        if (mouseEvent == Define.MouseEvent.LeftClick)
            Debug.Log("Attack!");
    }

    void OnKeyEvent(Define.KeyEvent keyEvent, Vector3 dir)
    {
        if (_state == Define.State.Die)
            return;

        switch (keyEvent)
        {
            case Define.KeyEvent.None:
                State = Define.State.Idle;
                break;
            case Define.KeyEvent.WASD:
                Move(_walkSpeed, Define.State.Walking, dir);
                break;
            case Define.KeyEvent.ShiftWASD:
                Move(_runSpeed, Define.State.Running, dir);
                break;
            case Define.KeyEvent.SpaceBar:
                Debug.Log("Jump!");
                break;
        }
    }
    #endregion

    void Move(float speed, Define.State state, Vector3 velocity)
    {
        if (velocity == Vector3.zero)
            return;

        // 방향
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), _angularSpeed * Time.deltaTime);

        GetComponent<CharacterController>().Move(velocity * speed * Time.deltaTime);
        State = state;
    }
}
