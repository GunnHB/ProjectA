using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class UIPanelBase : UIBase
{
    protected Sequence _panelSequence;

    public override void Init()
    {
        transform.SetParent(UIManager.Instance.PanelCanvas.transform);

        base.Init();

        _uiType = UIType.PANEL;
    }

    private void Start()
    {
        RectTransform rect = (RectTransform)transform;

        _panelSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .OnStart(() =>
        {
            UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            rect.localScale = new Vector3(.9f, .9f, .9f);

            Debug.Log("되는겨 안되는겨");
        })
        .Append(rect.DOScale(1f, .1f).SetEase(Ease.OutQuad))
        .Join(UIManager.Instance.PanelCanvas.GetComponent<CanvasGroup>().DOFade(1f, .1f))
        .SetDelay(.5f);
    }
}
