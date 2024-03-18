using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : SingletonObject<UIManager>
{
    private const string CANVAS_HUD = "HUDCanvas";
    private const string CANVAS_PANEL = "PanelCanvas";
    private const string CANVAS_POPUP = "PopupCanvas";
    private const string CANVAS_FLOATING = "FloatingCanvas";
    private const string CANVAS_FADE = "FadeCanvas";

    private Canvas _hudCanvas;
    private Canvas _panelCanvas;
    private Canvas _popupCanvas;
    private Canvas _floatingCanvas;
    private Canvas _fadeCanvas;

    public Canvas HUDCanvas { get => GetCanvasByProperty(ref _hudCanvas, CANVAS_HUD); }
    public Canvas PanelCanvas { get => GetCanvasByProperty(ref _panelCanvas, CANVAS_PANEL); }
    public Canvas PopupCanvas { get => GetCanvasByProperty(ref _popupCanvas, CANVAS_POPUP); }
    public Canvas FloatingCanvas { get => GetCanvasByProperty(ref _floatingCanvas, CANVAS_FLOATING); }
    public Canvas FadeCanvas { get => GetCanvasByProperty(ref _fadeCanvas, CANVAS_FADE); }

    private List<Canvas> _canvasList = new();

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// UI 열기 
    /// </summary>
    public T OpenUI<T>() where T : UIBase
    {
        var openedUI = GetUI<T>();

        if (openedUI == null)
            openedUI = LoadUI<T>();

        return openedUI;
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
            Debug.Log("there is no canvas");
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

        // 에셋번들에서 불러오기
        // 불러오는 에셋의 이름이 같아야 함둥
        var prefab = AssetBundleManager.Instance.GetUIBundle().LoadAsset<GameObject>(typeof(T).Name);

        if (prefab == null)
            return null;

        var ui = Instantiate(prefab).GetComponent<T>();

        if (ui == null)
            return null;

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
        // else if(typeof(T).BaseType.Equals(typeof()))
        else if (typeof(T).BaseType.Equals(typeof(UIFadeBase)))
            return FadeCanvas;
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

    /// <summary>
    /// 해당 UI 닫음
    /// </summary>
    public void CloseUI<T>(T ui) where T : UIBase
    {
        ui.PrevClose();
        ui.Close();
    }

    /// <summary>
    /// 콜백 실행 후 UI 닫기
    /// </summary>
    public void CloseUI<T>(T ui, UnityAction callback) where T : UIBase
    {
        callback?.Invoke();

        ui.PrevClose();
        ui.Close();
    }

    /// <summary>
    /// 캔버스의 모든 UI 닫음 
    /// </summary>
    public void CloseAllUI(Canvas canvas)
    {
        for (int index = 0; index < canvas.transform.childCount; index++)
        {
            var item = canvas.transform.GetChild(index);
            var uiItem = item.GetComponent<UIBase>();

            if (item != null && uiItem != null)
                CloseUI(uiItem);
        }
    }

    /// <summary>
    /// 어느 캔버스든 ui가 열렸는지 확인 (허드 제외)
    /// </summary>
    /// <returns></returns>
    public bool IsOpenAnyUIAllCanvas()
    {
        SetCanvasList();

        foreach (var canvas in _canvasList)
        {
            if (canvas == HUDCanvas || canvas == null)
                continue;

            for (int index = 0; index < canvas.transform.childCount; index++)
            {
                var ui = canvas.transform.GetChild(index);

                if (ui == null)
                    continue;

                if (ui.TryGetComponent(out UIBase uiBase))
                {
                    if (!uiBase.IsClose)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 모든 캔버스를 뒤져 열린 ui를 찾아 끄기 (허드 제외) 
    /// </summary>
    public void CloseTopUIByAllCanvas()
    {
        SetCanvasList();

        foreach (var canvas in _canvasList)
        {
            if (canvas == HUDCanvas)
                continue;

            if (IsCloseTopUI(canvas))
                return;
        }
    }

    /// <summary>
    /// 캔버스의 가장 상단 ui를 닫았는지 확인
    /// </summary>
    public bool IsCloseTopUI(Canvas canvas)
    {
        if (canvas == null)
            return false;

        for (int index = 0; index < canvas.transform.childCount; index++)
        {
            var ui = canvas.transform.GetChild(index);

            if (ui == null)
                continue;

            if (ui.TryGetComponent(out UIBase uiBase))
            {
                CloseUI(uiBase);
                return true;
            }
        }

        return false;
    }

    private void SetCanvasList()
    {
        _canvasList.Clear();
        _canvasList.AddRange(new List<Canvas>() { PopupCanvas, FloatingCanvas, PanelCanvas });
    }
}
