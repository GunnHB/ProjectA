using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIManager : SingletonObject<UIManager>
{
    private const string CANVAS_HUD = "HUDCanvas";
    private const string CANVAS_PANEL = "PanelCanvas";
    private const string CANVAS_POPUP = "PopupCanvas";

    private Canvas _hudCanvas;
    private Canvas _panelCanvas;
    private Canvas _popupCanvas;

    public Canvas HUDCanvas { get => GetCanvasByProperty(ref _panelCanvas, CANVAS_HUD); }
    public Canvas PanelCanvas { get => GetCanvasByProperty(ref _panelCanvas, CANVAS_PANEL); }
    public Canvas PopupCanvas { get => GetCanvasByProperty(ref _popupCanvas, CANVAS_POPUP); }

    protected override void Awake()
    {
        base.Awake();
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

        // LoadFromFile();

        // 에셋번들에서 불러오기
        // 불러오는 에셋의 이름이 같아야 함둥
        var prefab = AssetBundleManager.Instance.UIBundle.LoadAsset<GameObject>(typeof(T).Name);

        if (prefab == null)
            return null;

        var ui = Instantiate(prefab).GetComponent<T>();

        if (ui == null)
            return null;

        ui.transform.SetParent(canvas.transform);

        return ui;
    }

    private Canvas GetCanvas<T>() where T : UIBase
    {
        if (typeof(T).BaseType.Equals(typeof(UIHUDBase)))
            return HUDCanvas;
        else if (typeof(T).BaseType.Equals(typeof(UIPanelBase)))
            return PanelCanvas;
        else if (typeof(T).BaseType.Equals(typeof(UIPopupBase)))
            return PopupCanvas;
        else
            return null;
    }

    private Canvas GetCanvasByProperty(ref Canvas canvas, string canvasName)
    {
        if (canvas == null)
        {
            var obj = GameObject.Find(canvasName);

            if (obj != null)
                canvas = obj.GetComponent<Canvas>();
        }

        return canvas;
    }
}
