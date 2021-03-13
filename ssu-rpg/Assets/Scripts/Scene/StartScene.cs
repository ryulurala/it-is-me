﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    protected override void OnAwake()
    {
        StartCoroutine(threeSeconds());
    }

    public override void Clear()
    {
        Debug.Log("StartScene Clear");
    }

    IEnumerator threeSeconds()
    {
        yield return new WaitForSeconds(2.0f);
        // 1초 후
        Manager.Scene.LoadScene(Define.Scene.World);
    }
}
