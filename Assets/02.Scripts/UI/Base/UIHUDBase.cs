using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUDBase : UIBase
{
    public override void Init()
    {
        base.Init();

        _uiType = UIType.HUD;
    }
}
