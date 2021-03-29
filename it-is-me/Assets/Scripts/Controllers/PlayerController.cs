﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField] float _walkSpeed = 5f;
    [SerializeField] float _runSpeed = 10f;
    [SerializeField] float _angularSpeed = 30f;
    [SerializeField] Vector3 _velocity;

    public Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    anim.CrossFade("Die", 0.25f);
                    break;
                case Define.State.Idle:
                    anim.CrossFade("Idle", 0.25f);
                    break;
                case Define.State.Walking:
                    anim.CrossFade("Walk", 0.01f);
                    break;
                case Define.State.Running:
                    anim.CrossFade("Run", 0.01f);
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
    void UpdateWalking()
    {
        if (_velocity == Vector3.zero)
            State = Define.State.Idle;
    }
    void UpdateRunning()
    {
        if (_velocity == Vector3.zero)
            State = Define.State.Idle;
    }
    void UpdateAttack()
    {

    }

    #endregion

    #region Mobile
    void OnPadEvent(Define.PadEvent padEvent, Vector3 dir)
    {
        if (_state == Define.State.Die)
            return;

        _velocity = dir;
        switch (padEvent)
        {
            case Define.PadEvent.Dragging:
                Move(_walkSpeed, Define.State.Walking);
                break;
            case Define.PadEvent.RunButton:
                Move(_runSpeed, Define.State.Running);
                break;
            case Define.PadEvent.AttackButton:
                Debug.Log("Attack!");
                break;
            case Define.PadEvent.JumpButton:
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

        _velocity = dir;
        switch (keyEvent)
        {
            case Define.KeyEvent.WASD:
                Move(_walkSpeed, Define.State.Walking);
                break;
            case Define.KeyEvent.ShiftWASD:
                Move(_runSpeed, Define.State.Running);
                break;
            case Define.KeyEvent.SpaceBar:
                Debug.Log("Jump!");
                break;
        }
    }
    #endregion

    void Move(float speed, Define.State state)
    {
        if (_velocity == Vector3.zero)
            return;

        // 방향
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_velocity), _angularSpeed * Time.deltaTime);

        GetComponent<CharacterController>().SimpleMove(_velocity * speed);
        State = state;
    }
}
