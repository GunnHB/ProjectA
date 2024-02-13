using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected enum UIType
    {
        NONE = -1,
        HUD,
        PANEL,
        POPUP,
    }

    protected UIType _uiType;

    public abstract void Init();
}
