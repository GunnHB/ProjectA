using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIManager : SingletonObject<UIManager>
{
    private AssetBundle _loadedAssetBundle;

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

    private void Start()
    {
        _loadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "uibundle"));

        if (_loadedAssetBundle == null)
        {
            Debug.Log("fail to load asset bundle!!!");
            return;
        }
    }

    /// <summary>
    /// 하이어라키에 올라간 특정 UI 찾기 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUI<T>() where T : UIBase
    {
        var canvas = GetCanvas<T>();

        if (canvas == null)
        {
            Debug.Log("there is no cnavas");
            return null;
        }

        for (int index = 0; index < canvas.transform.childCount; index++)
        {
            var childObj = canvas.transform.GetChild(index);

            if (childObj.TryGetComponent(out T uiCompo))
                return uiCompo;
            else
                continue;
        }

        return null;
    }

    /// <summary>
    /// 특정 UI가 하이어라키에 올라와 있는지 확인
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsOpenedUI<T>() where T : UIBase
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
    public T LoadUI<T>() where T : UIBase
    {
        var canvas = GetCanvas<T>();

        if (canvas == null)
        {
            Debug.Log("there is no canvas");
            return null;
        }
        else if (_loadedAssetBundle == null)
        {
            Debug.Log("fail to load assetbundle!");
            return null;
        }

        // 에셋번들에서 불러오기 
        return _loadedAssetBundle.LoadAsset<T>(typeof(T).Name);
    }

    private Canvas GetCanvas<T>() where T : UIBase
    {
        if (typeof(T).BaseType.Equals(typeof(HUDBase)))
            return HUDCanvas;
        else
            return null;
    }
}
