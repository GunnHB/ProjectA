using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonObject<UIManager>
{
    private Canvas _hudCanvas;

    public Canvas HUDCanvas
    {
        get; set;
        // get
        // {
        //     if (_hudCanvas == null)

        // }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 하이어라키에 올라간 특정 UI 찾기 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOpenedUI<T>() where T : UIBase
    {
        // if (typeof(T) == typeof(HUDBase))
        //     Debug.Log("uibase 같음?? " + typeof(T));
        // else if (typeof(T).BaseType == typeof(HUDBase))
        //     Debug.Log("아니면 여기랑 같음?? " + typeof(T).BaseType);

        // return null;

        if (!IsOpenedUI<T>())
        {
            // 프리팹 불러와주기
        }
        else
        {

        }

        return null;
    }

    /// <summary>
    /// 특정 UI가 열렸는지 확인
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private bool IsOpenedUI<T>() where T : UIBase
    {
        return false;

        if (typeof(T).BaseType.Equals(typeof(HUDBase)))
        {

        }
    }

    /// <summary>
    /// UI 프리팹 불러오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T OpenUI<T>() where T : UIBase
    {
        return null;
    }
}
