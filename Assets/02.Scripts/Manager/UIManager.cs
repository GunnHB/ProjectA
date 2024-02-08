using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class UIManager : SingletonObject<UIManager>
{
    private Canvas _hudCanvas;

    public Canvas HUDCanvas
    {
        get
        {
            if (_hudCanvas == null)
            {
                var canvasObj = GameObject.Find("HUDCanvas");

                if (canvasObj != null)
                    _hudCanvas = canvasObj.GetComponent<Canvas>();
            }

            return _hudCanvas;
        }
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
        var canvas = GetCanvas<T>();

        if (canvas == null)
        {
            Debug.Log("there is no canvas");
            return false;
        }

        // 최상단의 ui를 확인함
        for (int index = 0; index < canvas.transform.childCount; index++)
        {
            var childObj = canvas.transform.GetChild(index);

            if (childObj.TryGetComponent(out T uiCompo))
                return true;
            else
                continue;
        }

        return false;
    }

    /// <summary>
    /// UI 프리팹 불러오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T OpenUI<T>() where T : UIBase
    {
        var canvas = GetCanvas<T>();

        if (canvas == null)
        {
            Debug.Log("there is no canvas");
            return null;
        }

        return null;
    }

    private Canvas GetCanvas<T>() where T : UIBase
    {
        if (typeof(T).BaseType.Equals(typeof(HUDBase)))
            return HUDCanvas;
        else
            return null;
    }
}
