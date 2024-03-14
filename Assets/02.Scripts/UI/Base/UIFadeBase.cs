using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIFadeBase : UIBase
{
    protected Image _fadeImage;

    public override void Init()
    {
        transform.SetParent(UIManager.Instance.FadeCanvas.transform);

        base.Init();

        _uiType = UIType.Fade;
    }

    public void Open(float alpha = 0f)
    {
        if (_fadeImage == null)
            return;

        _fadeImage.gameObject.SetActive(true);
        // 이미지 알파 값 세팅
        SetFade(alpha);
    }

    public void SetFade(float alpha)
    {
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, alpha);
    }

    public void SetRayTarget(bool isActive)
    {
        _fadeImage.raycastTarget = isActive;
    }
}
