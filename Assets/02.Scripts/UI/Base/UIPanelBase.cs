using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase
{
    public override void Init()
    {
        base.Init();

        _uiType = UIType.PANEL;
    }
}
