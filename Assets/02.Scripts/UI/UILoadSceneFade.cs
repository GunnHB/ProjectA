using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UILoadSceneFade : UIFadeBase
{


    public override void Init()
    {
        base.Init();

        if (_fadeImage == null)
        {
            if (GetBG() != null && GetBG().TryGetComponent(out Image bgImage))
            {
                _fadeImage = bgImage;
                _fadeImage.gameObject.SetActive(false);
            }
        }
    }
}
