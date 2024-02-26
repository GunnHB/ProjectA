using System;
using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    protected enum UIType
    {
        NONE = -1,
        HUD,
        PANEL,
        POPUP,
    }

    [BoxGroup("[Common]"), SerializeField]
    protected bool _needBlur;
    [BoxGroup("[Common]"), SerializeField]
    protected UIType _uiType;

    private GameObject _blurObj;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        SetBlur();
        SetAnchor();
    }

    private void SetAnchor()
    {
        var rect = this.transform as RectTransform;

        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(.5f, .5f);

        rect.offsetMin = new Vector2(0f, 0f);
        rect.offsetMax = new Vector3(0f, 0f);
    }

    private void SetBlur()
    {
        _blurObj = transform.Find("Blur").gameObject;

        if (_blurObj != null && !_needBlur)
            _blurObj.SetActive(false);
    }

    public virtual void Close()
    {
        Destroy(this.gameObject);
    }
}
