using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase
{
    public override void Init()
    {
        transform.SetParent(UIManager.Instance.PanelCanvas.transform);

        base.Init();

        _uiType = UIType.PANEL;
    }
}
