using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUDBase : UIBase
{
    public override void Init()
    {
        transform.SetParent(UIManager.Instance.HUDCanvas.transform);

        base.Init();

        _uiType = UIType.HUD;
    }
}
