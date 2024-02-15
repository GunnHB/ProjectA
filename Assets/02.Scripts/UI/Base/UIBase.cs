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
        _blurObj = transform.Find("Blur").gameObject;

        if (_blurObj != null && !_needBlur)
            _blurObj.SetActive(false);
    }
}
