using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupBase : UIBase
{
    public override void Init()
    {
        transform.SetParent(UIManager.Instance.PopupCanvas.transform);

        base.Init();

        _uiType = UIType.POPUP;
    }
}
