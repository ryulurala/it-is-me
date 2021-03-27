﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance;
    static Manager Instance { get { if (_instance == null) Init(); return _instance; } }

    #region core
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Awake()
    {
        _input.OnAwake();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        GameObject go = GameObject.Find("@Manager");
        if (go == null)
            go = new GameObject() { name = "@Manager" };

        // Scene이 이동해도 삭제 [X]
        DontDestroyOnLoad(go);
        _instance = go.GetOrAddComponent<Manager>();

        _instance._pool.Init();
    }

    public static void Clear()
    {
        Input.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
